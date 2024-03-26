using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Utility;
using Kris.Client.Core.Messages;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.ViewModels.Utility;
using Kris.Client.Views;
using MediatR;

namespace Kris.Client.ViewModels;

public abstract partial class PageViewModelBase : ObservableValidator
{
    protected readonly IMediator _mediator;
    protected readonly IRouterService _navigationService;
    protected readonly IMessageService _messageService;
    protected readonly IPopupService _popupService;
    protected readonly IAlertService _alertService;

    [ObservableProperty]
    protected Dictionary<string, string> errorMessages = new Dictionary<string, string>();

    public PageViewModelBase(IMediator mediator, IRouterService navigationService, IMessageService messageService,
        IPopupService popupService, IAlertService alertService)
    {
        _mediator = mediator;
        _navigationService = navigationService;
        _messageService = messageService;
        _popupService = popupService;
        _alertService = alertService;
    }

    [RelayCommand]
    protected virtual async Task OnMapButtonClicked() => await _navigationService.GoToAsync(nameof(MapView), RouterNavigationType.ReplaceUpward);
    [RelayCommand]
    protected virtual async Task OnMenuButtonClicked() => await _navigationService.GoToAsync(nameof(MenuView), RouterNavigationType.ReplaceUpward);

    protected virtual async Task LogoutUser()
    {
        _messageService.Send(new LogoutMessage());
        await _mediator.Send(new LogoutUserCommand(), CancellationToken.None);
        await _navigationService.GoToAsync(nameof(LoginView), RouterNavigationType.ReplaceUpward);
    }

    protected virtual void Cleanup()
    {
        ErrorMessages.Clear();
        OnPropertyChanged(nameof(ErrorMessages));
    }

    protected new bool ValidateAllProperties()
    {
        ErrorMessages.Clear();
        base.ValidateAllProperties();

        if (HasErrors)
        {
            foreach (var error in GetErrors())
            {
                var property = error.MemberNames.First();
                var message = error.ErrorMessage;

                var added = ErrorMessages.TryAdd(property, message);
                if (!added)
                {
                    var oldMessage = ErrorMessages[property];
                    ErrorMessages[property] = $"{oldMessage}\n{message}";
                }
            }
        }

        OnPropertyChanged(nameof(ErrorMessages));
        return HasErrors;
    }

    protected void AddCustomError(string name, string message)
    {
        var added = ErrorMessages.TryAdd(name, message);
        if (!added)
        {
            var oldMessage = ErrorMessages[name];
            ErrorMessages[name] = $"{oldMessage}\n{message}";
        }
        OnPropertyChanged(nameof(ErrorMessages));
    }

    protected async Task<T> MediatorSendLoadingAsync<T>(IRequest<T> request, CancellationToken ct)
    {
        Action closeLoadingPopup = null;
        var loadingPopupTask = _popupService.ShowPopupAsync<LoadingPopupViewModel>(vm => closeLoadingPopup = vm.Close);
        var result = await _mediator.Send(request, ct);

        closeLoadingPopup();
        await loadingPopupTask;
        return result;
    }

    protected async Task<T> MediatorSendAsync<T>(IRequest<T> request, CancellationToken ct)
    {
        return await _mediator.Send(request, ct);
    }
}
