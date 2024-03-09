﻿using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Events;
using Kris.Client.Components.Events;
using Kris.Client.Components.Map;
using Kris.Client.Connection.Hubs;
using Kris.Client.Connection.Hubs.Events;
using Kris.Client.Core.Listeners;
using Kris.Client.Core.Listeners.Events;
using Kris.Client.Core.Messages;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Utility;
using Kris.Client.ViewModels.Popups;
using Kris.Client.Views;
using Kris.Common.Extensions;
using MediatR;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;

namespace Kris.Client.ViewModels.Views;

public sealed partial class MapViewModel : PageViewModelBase
{
    private readonly IPopupService _popupService;
    private readonly IKrisMapObjectFactory _krisMapObjectFactory;
    private readonly ICurrentPositionListener _selfPositionListener;
    private readonly IUserPositionsListener _othersPositionListener;
    private readonly IMapObjectsListener _mapObjectsListener;
    private readonly IMessageReceiver _messageReceiver;

    [ObservableProperty]
    private MapSpan _currentRegion;
    [ObservableProperty]
    private MoveToRegionRequest _moveToRegion = new MoveToRegionRequest();

    [ObservableProperty]
    private ObservableCollection<KrisMapPinViewModel> _allMapPins = new ObservableCollection<KrisMapPinViewModel>();

    private CancellationTokenSource _selfPositionCTS;
    private Task _selfPositionTask;
    private CancellationTokenSource _othersPositionCTS;
    private Task _othersPositionTask;
    private CancellationTokenSource _mapObjectsCTS;
    private Task _mapObjectsTask;

    public MapViewModel(IPopupService popupService, IKrisMapObjectFactory krisMapObjectFactory, ICurrentPositionListener currentPositionListener,
        IUserPositionsListener userPositionsListener, IMapObjectsListener mapObjectsListener, IMessageReceiver messageReceiver,
        IMediator mediator, IRouterService navigationService, IMessageService messageService, IAlertService alertService)
        : base(mediator, navigationService, messageService, alertService)
    {
        _popupService = popupService;
        _krisMapObjectFactory = krisMapObjectFactory;
        _selfPositionListener = currentPositionListener;
        _othersPositionListener = userPositionsListener;
        _mapObjectsListener = mapObjectsListener;
        _messageReceiver = messageReceiver;

        _messageService.Register<LogoutMessage>(this, OnLogout);
        _messageService.Register<CurrentSessionChangedMessage>(this, RestartPositionListeners);
        _messageService.Register<ConnectionSettingsChangedMessage>(this, RestartPositionListeners);
    }

    // HANDLERS
    [RelayCommand]
    private async Task OnAppearing()
    {
        StartBackgroudListeners();
        await StartMessageListenerAsync();
    }
    [RelayCommand]
    private async Task OnMapLoaded() => await MoveToCurrentRegionAsync();
    [RelayCommand]
    private async Task OnCurrentPositionButtonClicked() => await MoveToCurrentPositionAsync();
    [RelayCommand]
    private async Task OnMapLongClicked(MapLongClickedEventArgs e) => await ShowCreateMapPointPopupAsync(e.Location);
    public async Task OnKrisPinClicked(KrisMapPin sender, PinClickedEventArgs e)
    {
        e.HideInfoWindow = true;
        await ShowEditMapPointPopupAsync(sender);
    }
    [RelayCommand]
    private async Task OnLogoutButtonClicked() => await LogoutUser();


    private void OnSelfPositionPositionChanged(object sender, LocationEventArgs e)
    {
        var userPin = _krisMapObjectFactory.CreateMyPositionPin(e.UserId, e.UserName, e.Location);
        var oldUserPin = AllMapPins.FirstOrDefault(p => p.Id == e.UserId);
        if (oldUserPin != null) AllMapPins.Remove(oldUserPin);
        AllMapPins.Add(userPin);
    }

    private async void OnSelfPositionErrorOccured(object sender, ResultEventArgs e)
    {
        if (e.Result.HasError<ServiceDisabledError>())
        {
            await _alertService.ShowToastAsync("GPS service is disabled");
        }
        else if (e.Result.HasError<ServicePermissionError>())
        {
            await _alertService.ShowToastAsync("GPS service is not permitted");
        }
        else if (e.Result.HasError<UnauthorizedError>())
        {
            await _alertService.ShowToastAsync("Login expired");
            await LogoutUser();
        }
        else
        {
            await _alertService.ShowToastAsync(e.Result.Errors.FirstMessage());
            OnLogout(this, null);
        }
    }

    private void OnOthersPositionPositionChanged(object sender, UserPositionsEventArgs e)
    {
        var userPins = e.Positions.Select(_krisMapObjectFactory.CreateUserPositionPin);
        AllMapPins = userPins.UnionBy(AllMapPins, pin => pin.Id).ToObservableCollection();
    }

    private async void OnOthersPositionErrorOccured(object sender, ResultEventArgs e)
    {
        if (e.Result.HasError<UnauthorizedError>())
        {
            await _alertService.ShowToastAsync("Login expired");
            await LogoutUser();
        }
        else
        {
            await _alertService.ShowToastAsync(e.Result.Errors.FirstMessage());
            OnLogout(this, null);
        }
    }

    private void OnMapObjectsChanged(object sender, MapObjectsEventArgs e)
    {
        var pointPins = e.MapPoints.Select(_krisMapObjectFactory.CreateMapPoint);
        AllMapPins = pointPins.UnionBy(AllMapPins, pin => pin.Id).ToObservableCollection();
    }

    private async void OnMapObjectsErrorOccured(object sender, ResultEventArgs e)
    {
        if (e.Result.HasError<UnauthorizedError>())
        {
            await _alertService.ShowToastAsync("Login expired");
            await LogoutUser();
        }
        else
        {
            await _alertService.ShowToastAsync(e.Result.Errors.FirstMessage());
            OnLogout(this, null);
        }

    }

    private async void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        // TODO: Better notification
        if (Shell.Current.CurrentPage is ChatView) return;
        await _alertService.ShowToastAsync($"{e.SenderName}: {e.Body}");
    }


    private async void OnLogout(object sender, LogoutMessage message)
    {
        if (_selfPositionListener.IsListening)
        {
            _selfPositionCTS.Cancel();

            try
            {
                await _selfPositionTask;
                _selfPositionListener.PositionChanged -= OnSelfPositionPositionChanged;
                _selfPositionListener.ErrorOccured -= OnSelfPositionErrorOccured;
            }
            catch (TaskCanceledException) { }
            finally
            {
                _selfPositionCTS.Dispose();
            }
        }
        if (_othersPositionListener.IsListening)
        {
            _othersPositionCTS.Cancel();

            try
            {
                await _othersPositionTask;
                _othersPositionListener.PositionsChanged -= OnOthersPositionPositionChanged;
                _othersPositionListener.ErrorOccured -= OnOthersPositionErrorOccured;
            }
            catch (TaskCanceledException) { }
            finally
            {
                _othersPositionCTS.Dispose();
            }
        }
        if (_mapObjectsListener.IsListening)
        {
            _mapObjectsCTS.Cancel();

            try
            {
                await _mapObjectsTask;
                _mapObjectsListener.MapObjectsChanged -= OnMapObjectsChanged;
                _mapObjectsListener.ErrorOccured -= OnMapObjectsErrorOccured;
            }
            catch (TaskCanceledException) { }
            finally
            {
                _mapObjectsCTS.Dispose();
            }
        }
        if (_messageReceiver.IsConnected)
        {
            _messageReceiver.MessageReceived -= OnMessageReceived;
            await _messageReceiver.Disconnect();
        }

        AllMapPins.Clear();
    }

    private async void RestartPositionListeners(object sender, MessageBase message)
    {
        if (_selfPositionListener.IsListening)
        {
            _selfPositionCTS.Cancel();

            try
            {
                await _selfPositionTask;
            }
            catch (TaskCanceledException) { }
            finally
            {
                _selfPositionCTS.Dispose();
            }
        }
        _selfPositionCTS = new CancellationTokenSource();
        _selfPositionTask = _selfPositionListener.StartListening(_selfPositionCTS.Token);

        if (_othersPositionListener.IsListening)
        {
            _othersPositionCTS.Cancel();

            try
            {
                await _othersPositionTask;
            }
            catch (TaskCanceledException) { }
            finally
            {
                _othersPositionCTS.Dispose();
            }
        }
        _othersPositionCTS = new CancellationTokenSource();
        _othersPositionTask = _othersPositionListener.StartListening(_othersPositionCTS.Token);

        if (_mapObjectsListener.IsListening)
        {
            _mapObjectsCTS.Cancel();

            try
            {
                await _mapObjectsTask;
            }
            catch (TaskCanceledException) { }
            finally
            {
                _mapObjectsCTS.Dispose();
            }
        }
        _mapObjectsCTS = new CancellationTokenSource();
        _mapObjectsTask = _mapObjectsListener.StartListening(_mapObjectsCTS.Token);

        if (message is CurrentSessionChangedMessage)
        {
            AllMapPins.Clear();

            await _messageReceiver.Disconnect();
            await _messageReceiver.Connect();
        }
    }

    // CORE
    private void StartBackgroudListeners()
    {
        if (!_selfPositionListener.IsListening)
        {
            _selfPositionCTS = new CancellationTokenSource();
            _selfPositionListener.PositionChanged += OnSelfPositionPositionChanged;
            _selfPositionListener.ErrorOccured += OnSelfPositionErrorOccured;
            _selfPositionTask = _selfPositionListener.StartListening(_selfPositionCTS.Token);
        }
        if (!_othersPositionListener.IsListening)
        {
            _othersPositionCTS = new CancellationTokenSource();
            _othersPositionListener.PositionsChanged += OnOthersPositionPositionChanged;
            _othersPositionListener.ErrorOccured += OnOthersPositionErrorOccured;
            _othersPositionTask = _othersPositionListener.StartListening(_othersPositionCTS.Token);
        }
        if (!_mapObjectsListener.IsListening)
        {
            _mapObjectsCTS = new CancellationTokenSource();
            _mapObjectsListener.MapObjectsChanged += OnMapObjectsChanged;
            _mapObjectsListener.ErrorOccured += OnMapObjectsErrorOccured;
            _mapObjectsTask = _mapObjectsListener.StartListening(_mapObjectsCTS.Token);
        }
    }

    private async Task StartMessageListenerAsync()
    {
        if (!_messageReceiver.IsConnected)
        {
            await _messageReceiver.Connect();
            _messageReceiver.MessageReceived += OnMessageReceived;
        }
    }

    private async Task MoveToCurrentRegionAsync()
    {
        var query = new GetCurrentRegionQuery();
        var currentRegion = await _mediator.Send(query, CancellationToken.None);

        if (currentRegion != null)
        {
            MoveToRegion.Execute(currentRegion);
        }
    }

    private async Task MoveToCurrentPositionAsync()
    {
        var query = new GetCurrentPositionQuery();
        var currentPosition = await _mediator.Send(query, CancellationToken.None);

        if (currentPosition == null)
        {
            await _alertService.ShowToastAsync("No current position");
        }
        else
        {
            var newRegion = MapSpan.FromCenterAndRadius(currentPosition, CurrentRegion.Radius);
            MoveToRegion.Execute(newRegion);
        }
    }

    private async Task ShowCreateMapPointPopupAsync(Location location)
    {
        var query = new GetCurrentUserQuery();
        var currentUser = await _mediator.Send(query, CancellationToken.None);
        if (currentUser == null || !currentUser.SessionId.HasValue || !currentUser.UserType.HasValue)
        {
            await _alertService.ShowToastAsync("Must join a session to create map objects");
            return;
        }

        var resultEventArgs = await _popupService.ShowPopupAsync<CreateMapPointPopupViewModel>(vm =>
        {
            vm.Location = location;
        }) as ResultEventArgs<MapPointModel>;
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
            await _alertService.ShowToastAsync("Map point created");
            AllMapPins.Add(_krisMapObjectFactory.CreateMapPoint(result.Value));
        }
    }

    private async Task ShowEditMapPointPopupAsync(KrisMapPin pin)
    {
        var query = new GetCurrentUserQuery();
        var currentUser = await _mediator.Send(query, CancellationToken.None);
        if (currentUser == null || !currentUser.SessionId.HasValue || !currentUser.UserType.HasValue)
        {
            await _alertService.ShowToastAsync("Invalid user data");
            await LogoutUser();
        }

        var resultArgs = await _popupService.ShowPopupAsync<EditMapPointPopupViewModel>(vm =>
        {
            vm.Initialize(currentUser, pin);
        });
        if (resultArgs == null) return;

        if (resultArgs is UpdateResultEventArgs)
        {
            // TODO
        }
        else if (resultArgs is DeleteResultEventArgs)
        {
            var result = (resultArgs as DeleteResultEventArgs).Result;
            if (result.IsFailed)
            {
                if (result.HasError<UnauthorizedError>())
                {
                    await _alertService.ShowToastAsync("Login expired");
                    await LogoutUser();
                }
                else if (result.HasError<EntityNotFoundError>())
                {
                    await _alertService.ShowToastAsync("Point not found");
                }
                else
                {
                    await _alertService.ShowToastAsync(result.Errors.FirstMessage());
                }
            }
            else
            {
                await _alertService.ShowToastAsync("Point deleted");
                var pinToRemove = AllMapPins.FirstOrDefault(p => p.Id == pin.KrisId);
                AllMapPins.Remove(pinToRemove);
            }
        }
    }

    // MISC
}
