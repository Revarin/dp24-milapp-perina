using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Enums;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Utility;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Utility;
using Kris.Client.ViewModels.Items;
using Kris.Client.ViewModels.Popups;
using Kris.Client.Views;
using Kris.Common.Extensions;
using MediatR;
using System.Collections.ObjectModel;

namespace Kris.Client.ViewModels.Views;

public sealed partial class SessionSettingsViewModel : PageViewModelBase
{
    private readonly IPopupService _popupService;

    [ObservableProperty]
    private SessionItemViewModel _currentSession;
    [ObservableProperty]
    private ObservableCollection<SessionItemViewModel> _joinedSessions;
    [ObservableProperty]
    private ObservableCollection<SessionItemViewModel> _otherSessions;

    public SessionSettingsViewModel(IPopupService popupService,
        IMediator mediator, IRouterService navigationService, IAlertService alertService)
        : base(mediator, navigationService, alertService)
    {
        _popupService = popupService;
    }

    [RelayCommand]
    private async Task OnAppearing()
    {
        var ct = new CancellationToken();
        var query = new GetSessionsQuery();
        var result = await _mediator.Send(query, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>())
            {
                await _alertService.ShowToastAsync("Login expired");
                await _mediator.Send(new LogoutUserCommand(), ct);
                await _navigationService.GoToAsync(nameof(LoginView), RouterNavigationType.ReplaceUpward);
            }
            else
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
            }
        }
        else
        {
            if (result.Value.CurrentSession != null)
            {
                CurrentSession = new SessionItemViewModel(result.Value.CurrentSession, SessionItemType.Current);
                CurrentSession.SessionJoining += OnSessionJoining;
                CurrentSession.SessionLeaving += OnSessionLeaving;
            }

            JoinedSessions = result.Value.JoinedSessions.Select(s => new SessionItemViewModel(s, SessionItemType.Joined)).ToObservableCollection();
            foreach (var session in JoinedSessions)
            {
                session.SessionJoining += OnSessionJoining;
                session.SessionLeaving += OnSessionLeaving;
            }

            OtherSessions = result.Value.OtherSessions.Select(s => new SessionItemViewModel(s, SessionItemType.Other)).ToObservableCollection();
            foreach (var session in OtherSessions)
            {
                session.SessionJoining += OnSessionJoining;
                session.SessionLeaving += OnSessionLeaving;
            }
        }
    }

    [RelayCommand]
    private async Task OnCreateSessionClicked()
    {
        await _popupService.ShowPopupAsync<EditSessionPopupViewModel>();
    }

    private void OnSessionJoining(object sender, EntityIdEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnSessionLeaving(object sender, EntityIdEventArgs e)
    {
        throw new NotImplementedException();
    }
}
