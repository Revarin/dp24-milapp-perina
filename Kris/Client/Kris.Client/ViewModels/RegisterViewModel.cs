using Kris.Client.Common.Enums;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Views;
using MediatR;
using System.Windows.Input;

namespace Kris.Client.ViewModels;

public sealed class RegisterViewModel : ViewModelBase
{
    private string _login;
    public string Login
    {
        get { return _login; }
        set { SetPropertyValue(ref _login, value); }
    }
    private string _password;
    public string Password
    {
        get { return _password; }
        set { SetPropertyValue(ref _password, value); }
    }
    private string _passwordVerification;
    public string PasswordVerification
    {
        get { return _passwordVerification; }
        set { SetPropertyValue(ref _passwordVerification, value); }
    }

    public required ICommand BackClickedCommand { get; init; }
    public required ICommand RegisterClickedCommand { get; init; }

    public RegisterViewModel(IMediator mediator, IRouterService navigationService, IAlertService alertService)
        : base(mediator, navigationService, alertService)
    {
        BackClickedCommand = new Command(async _ => await OnBackClickedCommand());
        RegisterClickedCommand = new Command(async _ => await OnRegisterClickedCommand());
    }

    private async Task OnBackClickedCommand()
    {
        await _navigationService.GoToAsync(nameof(LoginView), RouterNavigationType.ReplaceUpward);
    }

    private async Task OnRegisterClickedCommand()
    {
        if (Password != PasswordVerification) return;

        var ct = new CancellationToken();
        var command = new RegisterUserCommand { Login = Login, Password = Password };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            _alertService.ShowToast("Registration failed");
        }
        else
        {
            _alertService.ShowToast("Registration succeeded");
        }
    }
}
