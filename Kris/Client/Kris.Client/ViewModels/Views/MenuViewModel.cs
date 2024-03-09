﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Utility;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Views;
using MediatR;

namespace Kris.Client.ViewModels.Views;

public sealed partial class MenuViewModel : PageViewModelBase
{
    [ObservableProperty]
    private bool _inSession;

    public MenuViewModel(IMediator mediator, IRouterService navigationService, IMessageService messageService, IAlertService alertService)
        : base(mediator, navigationService, messageService, alertService)
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

    // CORE
    private async Task GetCurrentSessionAsync()
    {
        var ct = new CancellationToken();
        var query = new GetCurrentUserQuery();
        var result = await _mediator.Send(query, ct);

        InSession = result.SessionId.HasValue;
    }
}
