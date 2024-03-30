using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using Kris.Client.ViewModels.Utility;
using MediatR;

namespace Kris.Client.ViewModels;

public abstract partial class FormViewModelBase : ObservableValidator
{
    protected readonly IMediator _mediator;
    protected readonly IPopupService _popupService;

    [ObservableProperty]
    protected Dictionary<string, string> errorMessages = new Dictionary<string, string>();

    protected FormViewModelBase(IMediator mediator, IPopupService popupService)
    {
        _mediator = mediator;
        _popupService = popupService;
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

        AddErrors();

        OnPropertyChanged(nameof(ErrorMessages));
        return HasErrors;
    }

    protected void AddErrors()
    {
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
