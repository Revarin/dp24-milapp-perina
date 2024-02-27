using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Utility;
using Kris.Client.Core.Services;
using Kris.Client.Views;
using MediatR;

namespace Kris.Client.ViewModels.Views;

public sealed partial class MenuViewModel : PageViewModelBase
{
    public MenuViewModel(IMediator mediator, IRouterService navigationService, IMessageService messageService, IAlertService alertService)
        : base(mediator, navigationService, messageService, alertService)
    {
    }

    [RelayCommand]
    private async Task OnSessionSettingsClicked()
    {
        await _navigationService.GoToAsync(nameof(SessionSettingsView), RouterNavigationType.PushUpward);
    }

    [RelayCommand]
    private async Task OnUserSettingsClicked()
    {
        await _navigationService.GoToAsync(nameof(UserSettingsView), RouterNavigationType.PushUpward);
    }
}
