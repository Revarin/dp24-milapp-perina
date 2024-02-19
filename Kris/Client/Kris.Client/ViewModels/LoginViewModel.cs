using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Enums;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Views;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Kris.Client.ViewModels;

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

    [RelayCommand]
    private async Task OnRegisterClicked()
    {
        await _navigationService.GoToAsync(nameof(RegisterView), RouterNavigationType.ReplaceUpward);
    }

    [RelayCommand]
    private async Task OnLoginClicked()
    {
        await _navigationService.GoToAsync(nameof(MapView), RouterNavigationType.ReplaceDownward);
        return;
        if (ValidateAllProperties()) return;

        var ct = new CancellationToken();
        var command = new LoginUserCommand { Login = Login, Password = Password };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            // TODO
            _alertService.ShowToast("Login failed");
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
