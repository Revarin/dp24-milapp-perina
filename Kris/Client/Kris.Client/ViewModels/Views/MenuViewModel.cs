using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Utility;
using Kris.Client.Core.Services;
using Kris.Client.Views;
using MediatR;

namespace Kris.Client.ViewModels.Views;

public sealed partial class MenuViewModel : ViewModelBase
{
    public MenuViewModel(IMediator mediator, IRouterService navigationService, IAlertService alertService)
        : base(mediator, navigationService, alertService)
    {
    }

    [RelayCommand]
    private async Task OnSessionSettingsClicked()
    {
        await _navigationService.GoToAsync(nameof(SessionSettingsView), RouterNavigationType.PushUpward);
    }
}
