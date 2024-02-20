using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Enums;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Views;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Kris.Client.ViewModels.Views;

public sealed partial class LoginViewModel : ViewModelBase
{
    [Required]
    [ObservableProperty]
    private string _login;
    [Required]
    [ObservableProperty]
    private string _password;

    public LoginViewModel(IMediator mediator, IRouterService navigationService, IAlertService alertService)
        : base(mediator, navigationService, alertService)
    {
    }

    protected override async Task InitAsync()
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

    [RelayCommand]
    private async Task OnRegisterClicked()
    {
        await _navigationService.GoToAsync(nameof(RegisterView), RouterNavigationType.ReplaceUpward);
    }

    [RelayCommand]
    private async Task OnLoginClicked()
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

    protected override void Cleanup()
    {
        Login = string.Empty;
        Password = string.Empty;

        base.Cleanup();
    }
}
