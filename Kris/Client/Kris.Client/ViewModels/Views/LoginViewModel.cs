using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Utility;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Views;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Kris.Client.ViewModels.Views;

public sealed partial class LoginViewModel : PageViewModelBase
{
    [Required]
    [ObservableProperty]
    private string _login;
    [Required]
    [ObservableProperty]
    private string _password;

    public LoginViewModel(IMediator mediator, IRouterService navigationService, IMessageService messageService, IAlertService alertService)
        : base(mediator, navigationService, messageService, alertService)
    {
    }

    // HANDLERS
    protected override async Task InitAsync() => await GetCurrentUserAsync();
    [RelayCommand]
    private async Task OnRegisterButtonClicked() => await _navigationService.GoToAsync(nameof(RegisterView), RouterNavigationType.PushUpward);
    [RelayCommand]
    private async Task OnLoginButtonClicked() => await LoginUserAsync();

    // CORE
    private async Task GetCurrentUserAsync()
    {
        var ct = new CancellationToken();
        var query = new GetCurrentUserQuery();
        var result = await _mediator.Send(query, ct);

        if (result == null) return;

        if (result.LoginExpiration > DateTime.UtcNow)
        {
            // TODO: Refresh token
            await _alertService.ShowToastAsync("Logged in");
            await _navigationService.GoToAsync(nameof(MapView), RouterNavigationType.ReplaceUpward);
        }
        else
        {
            Login = result.Login;
        }
    }

    private async Task LoginUserAsync()
    {
        if (ValidateAllProperties()) return;

        var ct = new CancellationToken();
        var command = new LoginUserCommand { Login = Login, Password = Password };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>())
            {
                AddCustomError(nameof(Password), "Invalid credentials");
            }
            else
            {
                await _alertService.ShowToastAsync("Server error");
            }
        }
        else
        {
            Cleanup();
            await _alertService.ShowToastAsync("Logged in");
            await _navigationService.GoToAsync(nameof(MapView), RouterNavigationType.ReplaceUpward);
        }
    }

    // MISC
    protected override void Cleanup()
    {
        Login = string.Empty;
        Password = string.Empty;

        base.Cleanup();
    }
}
