using Kris.Client.Common.Enums;
using Kris.Client.Core.Services;
using Kris.Client.Views;
using System.Windows.Input;

namespace Kris.Client.ViewModels;

public sealed class RegisterViewModel : ViewModelBase
{
    public required ICommand BackClickedCommand { get; init; }

    public RegisterViewModel(IRouterService navigationService, IAlertService alertService)
        : base(navigationService, alertService)
    {
        BackClickedCommand = new Command(async _ => await OnBackClickedCommand());
    }

    private async Task OnBackClickedCommand()
    {
        await _navigationService.GoToAsync(nameof(LoginView), RouterNavigationType.ReplaceUpward);
    }
}
