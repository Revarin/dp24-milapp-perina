using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Enums;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Events;
using Kris.Client.Common.Utility;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
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
                CurrentSession = new SessionItemViewModel(result.Value.CurrentSession, SessionItemType.Current, result.Value.UserType);
                CurrentSession.SessionJoining += OnSessionJoining;
                CurrentSession.SessionLeaving += OnSessionLeaving;
                CurrentSession.SessionEditing += OnSessionEditing;
            }

            JoinedSessions = result.Value.JoinedSessions.Select(s => new SessionItemViewModel(s, SessionItemType.Joined, result.Value.UserType))
                .ToObservableCollection();
            foreach (var session in JoinedSessions)
            {
                session.SessionJoining += OnSessionJoining;
                session.SessionLeaving += OnSessionLeaving;
                session.SessionEditing += OnSessionEditing;
            }

            OtherSessions = result.Value.OtherSessions.Select(s => new SessionItemViewModel(s, SessionItemType.Other, result.Value.UserType))
                .ToObservableCollection();
            foreach (var session in OtherSessions)
            {
                session.SessionJoining += OnSessionJoining;
                session.SessionLeaving += OnSessionLeaving;
                session.SessionEditing += OnSessionEditing;
            }
        }
    }

    [RelayCommand]
    private async Task OnCreateSessionClicked()
    {
        var resultEventArgs = await _popupService.ShowPopupAsync<EditSessionPopupViewModel>() as ResultEventArgs;
        if (resultEventArgs == null) return;

        var result = resultEventArgs.Result;

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>())
            {
                await LoginExpired();
            }
            else
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
            }
        }
        else
        {
            await _alertService.ShowToastAsync("Session created");
            await OnAppearing();
        }
    }

    private async void OnSessionJoining(object sender, EntityIdEventArgs eId)
    {
        var ePassword = await _popupService.ShowPopupAsync<PasswordPopupViewModel>() as PasswordEventArgs;
        if (ePassword == null) return;

        var ct = new CancellationToken();
        var command = new JoinSessionCommand { SessionId = eId.Id, Password = ePassword.Password };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<ForbiddenError>())
            {
                await _alertService.ShowToastAsync("Wrong password");
            }
            else if (result.HasError<UnauthorizedError>())
            {
                await LoginExpired();
            }
            else if (result.HasError<EntityNotFoundError>())
            {
                await _alertService.ShowToastAsync("Session not found, refreshing session list...");
                await OnAppearing();
            }
            else
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
            }
        }
        else
        {
            await _alertService.ShowToastAsync("Joined session");
            await OnAppearing();
        }
    }

    private async void OnSessionLeaving(object sender, EntityIdEventArgs e)
    {
        var ct = new CancellationToken();
        var command = new LeaveSessionCommand { SessionId = e.Id };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>())
            {
                await LoginExpired();
            }
            else if (result.HasError<BadOperationError>())
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
                await OnAppearing();
            }
            else
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
            }
        }
        else
        {
            await _alertService.ShowToastAsync("Left session");
            await OnAppearing();
        }
    }

    private async void OnSessionEditing(object sender, EntityIdEventArgs e)
    {
        // TODO: TEMP DELETE
        var ct = new CancellationToken();
        var command = new EndSessionCommand();
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>() || result.HasError<ForbiddenError>())
            {
                await LoginExpired();
            }
            else if (result.HasError<EntityNotFoundError>())
            {
                await _alertService.ShowToastAsync("Session not found, refreshing session list...");
                await OnAppearing();
            }
            else
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
            }
        }
        else
        {
            await _alertService.ShowToastAsync("Deleted session");
            await OnAppearing();
        }
    }
}
