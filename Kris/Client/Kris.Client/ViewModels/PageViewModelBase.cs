using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Utility;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Views;
using MediatR;

namespace Kris.Client.ViewModels;

public abstract partial class PageViewModelBase : ObservableValidator
{
    protected readonly IMediator _mediator;
    protected readonly IRouterService _navigationService;
    protected readonly IAlertService _alertService;

    public Task InitializationWork { get; private set; }

    [ObservableProperty]
    protected bool _isLoading;
    [ObservableProperty]
    protected Dictionary<string, string> errorMessages = new Dictionary<string, string>();

    public PageViewModelBase(IMediator mediator, IRouterService navigationService, IAlertService alertService)
    {
        _mediator = mediator;
        _navigationService = navigationService;
        _alertService = alertService;
    }

    // Source: https://www.reddit.com/r/dotnetMAUI/comments/16b2uy7/async_data_loading_on_page_load/
    public async void Init()
    {
        try
        {
            IsLoading = true;
            InitializationWork = InitAsync();
            await InitializationWork;
        }
        catch
        {
            throw;
        }
        finally
        {
            IsLoading = false;
        }
    }

    protected virtual Task InitAsync()
    {
        return Task.CompletedTask;
    }

    [RelayCommand]
    protected async Task GoToMap() => await _navigationService.GoToAsync(nameof(MapView), RouterNavigationType.ReplaceUpward);

    [RelayCommand]
    protected async Task GoToMenu() => await _navigationService.GoToAsync(nameof(MenuView), RouterNavigationType.ReplaceUpward);

    protected async Task LoginExpired()
    {
        await _alertService.ShowToastAsync("Login expired");
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
}
