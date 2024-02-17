using Kris.Client.Common.Enums;
using Kris.Client.Core.Services;
using Kris.Client.Views;
using MediatR;
using System.Windows.Input;

namespace Kris.Client.ViewModels;

public sealed class LoginViewModel : ViewModelBase
{
    public required ICommand RegisterClickedCommand { get; init; }

    public LoginViewModel(IMediator mediator, IRouterService navigationService, IAlertService alertService)
        : base(mediator, navigationService, alertService)
    {
        RegisterClickedCommand = new Command(async _ => await OnRegisterClicked());
    }

    private async Task OnRegisterClicked()
    {
        await _navigationService.GoToAsync(nameof(RegisterView), RouterNavigationType.ReplaceUpward);
    }
}
