﻿using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Enums;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Events;
using Kris.Client.Components.Events;
using Kris.Client.Core.Messages;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.ViewModels.Items;
using Kris.Client.ViewModels.Popups;
using Kris.Client.ViewModels.Utility;
using Kris.Common.Extensions;
using MediatR;
using System.Collections.ObjectModel;

namespace Kris.Client.ViewModels.Views;

public sealed partial class SessionSettingsViewModel : PageViewModelBase
{
    [ObservableProperty]
    private SessionItemViewModel _currentSession;
    [ObservableProperty]
    private ObservableCollection<SessionItemViewModel> _joinedSessions;
    [ObservableProperty]
    private ObservableCollection<SessionItemViewModel> _otherSessions;

    public SessionSettingsViewModel(IMediator mediator, IRouterService navigationService, IMessageService messageService, IPopupService popupService, IAlertService alertService)
        : base(mediator, navigationService, messageService, popupService, alertService)
    {
    }

    // HANDLERS
    [RelayCommand]
    private async Task OnAppearing() => await LoadSessionAsync();
    [RelayCommand]
    private async Task OnCreateSessionButtonClicked() => await ShowCreateSessionPopupAsync();
    private async void OnSessionJoining(object sender, EntityIdEventArgs e) => await JoinSessionAsync(e.Id);
    private async void OnSessionLeaving(object sender, EntityIdEventArgs e) => await LeaveSessionAsync(e.Id);
    private async void OnSessionEditing(object sender, EntityIdEventArgs e) => await ShowEditSessionPopupAsync(e.Id);

    // CORE
    private async Task LoadSessionAsync()
    {
        var ct = new CancellationToken();
        var query = new GetSessionsQuery();
        var result = await MediatorSendLoadingAsync(query, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>())
            {
                await _alertService.ShowToastAsync("Login expired");
                await LogoutUser();
            }
            if (result.HasError<ConnectionError>())
            {
                await _alertService.ShowToastAsync("No connection to server");
                await _navigationService.GoBackAsync();
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
            else
            {
                CurrentSession = null;
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

    private async Task ShowCreateSessionPopupAsync()
    {
        var resultEventArgs = await _popupService.ShowPopupAsync<CreateSessionPopupViewModel>() as ResultEventArgs;
        if (resultEventArgs == null) return;

        var result = resultEventArgs.Result;

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>())
            {
                await _alertService.ShowToastAsync("Login expired");
                await LogoutUser();
            }
            else
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
            }
        }
        else
        {
            await _alertService.ShowToastAsync("Session created");
            _messageService.Send(new CurrentSessionChangedMessage());
            await OnAppearing();
        }
    }

    private async Task JoinSessionAsync(Guid sessionId)
    {
        var ePassword = await _popupService.ShowPopupAsync<PasswordPopupViewModel>() as PasswordEventArgs;
        if (ePassword == null) return;

        var ct = new CancellationToken();
        var command = new JoinSessionCommand { SessionId = sessionId, Password = ePassword.Password };
        var result = await MediatorSendAsync(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<ForbiddenError>())
            {
                await _alertService.ShowToastAsync("Wrong password");
            }
            else if (result.HasError<UnauthorizedError>())
            {
                await _alertService.ShowToastAsync("Login expired");
                await LogoutUser();
            }
            else if (result.HasError<EntityNotFoundError>())
            {
                await _alertService.ShowToastAsync("Session not found, refreshing session list...");
                await OnAppearing();
            }
            else if (result.HasError<ConnectionError>())
            {
                await _alertService.ShowToastAsync("No connection to server");
            }
            else
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
            }
        }
        else
        {
            await _alertService.ShowToastAsync("Joined session");
            _messageService.Send(new CurrentSessionChangedMessage());
            await OnAppearing();
        }
    }

    private async Task LeaveSessionAsync(Guid sessionId)
    {
        var confirmation = await _popupService.ShowPopupAsync<ConfirmationPopupViewModel>(vm => vm.Message = "Leave session?") as ConfirmationEventArgs;
        if (confirmation == null || !confirmation.IsConfirmed) return;

        var ct = new CancellationToken();
        var command = new LeaveSessionCommand { SessionId = sessionId };
        var result = await MediatorSendAsync(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>())
            {
                await _alertService.ShowToastAsync("Login expired");
                await LogoutUser();
            }
            else if (result.HasError<BadOperationError>())
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
                await OnAppearing();
            }
            else if (result.HasError<ConnectionError>())
            {
                await _alertService.ShowToastAsync("No connection to server");
            }
            else
            {
                await _alertService.ShowToastAsync(result.Errors.FirstMessage());
            }
        }
        else
        {
            await _alertService.ShowToastAsync("Left session");
            _messageService.Send(new CurrentSessionChangedMessage());
            await OnAppearing();
        }
    }

    private async Task ShowEditSessionPopupAsync(Guid sessionId)
    {
        var query = new GetCurrentUserQuery();
        var currentUser = await MediatorSendAsync(query, CancellationToken.None);
        if (currentUser == null || !currentUser.SessionId.HasValue || !currentUser.UserType.HasValue)
        {
            await _alertService.ShowToastAsync("Invalid user data");
            await LogoutUser();
        }

        var resultArgs = await _popupService.ShowPopupAsync<EditSessionPopupViewModel>(async vm =>
        {
            vm.SessionId = sessionId;
            vm.UserType = currentUser.UserType.Value;
            await vm.LoadSessionDetailAsync();
        });
        if (resultArgs == null) return;

        if (resultArgs is LoadResultEventArgs<SessionDetailModel> loadResult)
        {
            var result = loadResult.Result;
            if (result.IsFailed)
            {
                if (result.HasError<UnauthorizedError>())
                {
                    await _alertService.ShowToastAsync("Login expired");
                    await LogoutUser();
                }
                else
                {
                    await _alertService.ShowToastAsync(result.Errors.FirstMessage());
                }
            }
        }
        else if (resultArgs is DeleteResultEventArgs deleteResult)
        {
            var result = deleteResult.Result;
            if (result.IsFailed)
            {
                if (result.HasError<UnauthorizedError>())
                {
                    await _alertService.ShowToastAsync("Login expired");
                    await LogoutUser();
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
                await _alertService.ShowToastAsync("Session deleted");
                _messageService.Send(new CurrentSessionChangedMessage());
                await OnAppearing();
            }
        }
        else if (resultArgs is UpdateResultEventArgs updateResult)
        {
            var result = updateResult.Result;
            if (result.IsFailed)
            {
                if (result.HasError<UnauthorizedError>())
                {
                    await _alertService.ShowToastAsync("Login expired");
                    await LogoutUser();
                }
                else
                {
                    await _alertService.ShowToastAsync(result.Errors.FirstMessage());
                }
            }
            else
            {
                await _alertService.ShowToastAsync("Session updated");
                _messageService.Send(new CurrentSessionChangedMessage());
                await OnAppearing();
            }
        }
    }
}
