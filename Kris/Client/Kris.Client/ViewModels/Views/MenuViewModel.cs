using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Events;
using Kris.Client.Common.Utility;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.ViewModels.Utility;
using Kris.Client.Views;
using MediatR;

namespace Kris.Client.ViewModels.Views;

public sealed partial class MenuViewModel : PageViewModelBase
{
    [ObservableProperty]
    private bool _inSession;

    public MenuViewModel(IMediator mediator, IRouterService navigationService, IMessageService messageService, IPopupService popupService, IAlertService alertService)
        : base(mediator, navigationService, messageService, popupService, alertService)
    {
    }

    // HANDLERS
    [RelayCommand]
    private async Task OnAppearing() => await GetCurrentSessionAsync();
    [RelayCommand]
    private async Task OnSessionSettingsButtonClicked() => await _navigationService.GoToAsync(nameof(SessionSettingsView), RouterNavigationType.PushUpward);
    [RelayCommand]
    private async Task OnUserSettingsButtonClicked() => await _navigationService.GoToAsync(nameof(UserSettingsView), RouterNavigationType.PushUpward);
    [RelayCommand]
    private async Task OnContactsButtonClicked() => await _navigationService.GoToAsync(nameof(ContactsView), RouterNavigationType.PushUpward);
    [RelayCommand]
    private async Task OnMapSettingsButtonClicked() => await _navigationService.GoToAsync(nameof(MapSettingsView), RouterNavigationType.PushUpward);
    [RelayCommand]
    private async Task OnLogoutButtonClicked()
    {
        var confirmation = await _popupService.ShowPopupAsync<ConfirmationPopupViewModel>(vm => vm.Message = "Do you want to logout?") as ConfirmationEventArgs;
        if (confirmation == null || !confirmation.IsConfirmed) return;
        Common.Metrics.SentryMetrics.CounterIncrement("CorrectLogoutUser");
        await LogoutUser();
    }

    // CORE
    private async Task GetCurrentSessionAsync()
    {
        var query = new GetCurrentUserQuery();
        var result = await MediatorSendAsync(query, CancellationToken.None);

        InSession = result.SessionId.HasValue;
    }
}
