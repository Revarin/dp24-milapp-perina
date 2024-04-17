using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Enums;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Events;
using Kris.Client.Components.Events;
using Kris.Client.Components.Map;
using Kris.Client.Connection.Hubs;
using Kris.Client.Connection.Hubs.Events;
using Kris.Client.Converters;
using Kris.Client.Core.Background;
using Kris.Client.Core.Background.Events;
using Kris.Client.Core.Messages;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Data.Database;
using Kris.Client.Data.Providers;
using Kris.Client.Platforms.Background;
using Kris.Client.Utility;
using Kris.Client.ViewModels.Items;
using Kris.Client.ViewModels.Popups;
using Kris.Client.Views;
using Kris.Common.Enums;
using Kris.Common.Extensions;
using MediatR;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;

namespace Kris.Client.ViewModels.Views;

public sealed partial class MapViewModel : PageViewModelBase
{
    private readonly IMapSettingsDataProvider _mapSettingsDataProvider;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IKrisMapObjectFactory _krisMapObjectFactory;
    private readonly ICurrentPositionBackgroundHandler _currentPositionBackgroundHandler;
    private readonly IUserPositionsBackgroundHandler _userPositionsBackgroundHandler;
    private readonly IMapObjectsBackgroundHandler _mapObjectsBackgroundHandler;
    private readonly IMessageReceiver _messageReceiver;

    [ObservableProperty]
    private DisplayOrientation _displayOrientation;
    [ObservableProperty]
    private MapSpan _currentRegion;
    [ObservableProperty]
    private IViewRequest<MapSpan> _moveToRegion = new MoveToRegionRequest();
    [ObservableProperty]
    private LocationCoordinates _currentPosition = new LocationCoordinates();

    [ObservableProperty]
    private KrisMapStyle _krisMapStyle = null;
    [ObservableProperty]
    private ICollection<KrisMapPinViewModel> _allMapPins = new ObservableCollection<KrisMapPinViewModel>();

    private CancellationTokenSource _backgroundHandlersCTS;
    private Task _userPositionsBackgroundTask;
    private Task _mapObjectsBackgroundTask;

    private IMapTileRepository _mapTileRepository;
    private CoordinateSystem _coordinateSystem;
    private KrisMapType _krisMapType;

    public MapViewModel(IMapSettingsDataProvider mapSettingsDataProvider, IRepositoryFactory repositoryFactory, IKrisMapObjectFactory krisMapObjectFactory,
        ICurrentPositionBackgroundHandler currentPositionBackgroundHandler, IUserPositionsBackgroundHandler userPositionsBackgroundHandler,
        IMapObjectsBackgroundHandler mapObjectsBackgroundHandler, IMessageReceiver messageReceiver,
        IMediator mediator, IRouterService navigationService, IMessageService messageService, IPopupService popupService, IAlertService alertService)
        : base(mediator, navigationService, messageService, popupService, alertService)
    {
        _mapSettingsDataProvider = mapSettingsDataProvider;
        _repositoryFactory = repositoryFactory;
        _krisMapObjectFactory = krisMapObjectFactory;
        _currentPositionBackgroundHandler = currentPositionBackgroundHandler;
        _userPositionsBackgroundHandler = userPositionsBackgroundHandler;
        _mapObjectsBackgroundHandler = mapObjectsBackgroundHandler;
        _messageReceiver = messageReceiver;

        _messageService.Register<LogoutMessage>(this, OnLogout);
        _messageService.Register<CurrentSessionChangedMessage>(this, OnBackgroundContextChanged);
        _messageService.Register<ConnectionSettingsChangedMessage>(this, OnBackgroundContextChanged);
        _messageService.Register<MapSettingsChangedMessage>(this, async (sender, msg) => await LoadMapSettingsAsync(true));

        DisplayOrientation = DeviceDisplay.Current.MainDisplayInfo.Orientation;
        DeviceDisplay.Current.MainDisplayInfoChanged += MainDisplayInfoChanged;
    }


    // HANDLERS
    [RelayCommand]
    private async Task OnAppearing()
    {
        StartBackgroudListeners();
        await StartMessageListenerAsync();
        await LoadMapSettingsAsync(false);
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

    private void OnCurrentPositionChanged(object sender, UserPositionEventArgs e) => AddCurrentUserPositionToMap(e.Position);
    private void OnUserPositionsChanged(object sender, UserPositionsEventArgs e) => AddOtherUserPositionsToMap(e.Positions);
    private void OnMapObjectsChanged(object sender, MapObjectsEventArgs e) => AddMapObjectsToMap(e.MapPoints, e.DeletedMapPoints);
    private async void OnMessageReceived(object sender, MessageReceivedEventArgs e) => await ShowMessageNotification(e.Id, e.SenderName, e.Body);

    private void MainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
    {
        DisplayOrientation = e.DisplayInfo.Orientation;
    }

    // CORE
    private void StartBackgroudListeners()
    {
        if (_backgroundHandlersCTS == null)
        {
            _backgroundHandlersCTS = new CancellationTokenSource();
        }

        if (!_currentPositionBackgroundHandler.IsRunning)
        {
            _currentPositionBackgroundHandler.ReloadSettings = true;
            _currentPositionBackgroundHandler.CurrentPositionChanged += OnCurrentPositionChanged;
            _currentPositionBackgroundHandler.ErrorOccured += OnBackgroundHandlerErrorOccured;
            CurrentPositionBackgroundService.StartService();
            _currentPositionBackgroundHandler.IsRunning = true;
        }
        if (!_userPositionsBackgroundHandler.IsRunning)
        {
            _userPositionsBackgroundHandler.ReloadSettings = true;
            _userPositionsBackgroundHandler.UserPositionsChanged += OnUserPositionsChanged;
            _userPositionsBackgroundHandler.ErrorOccured += OnBackgroundHandlerErrorOccured;
            _userPositionsBackgroundTask = _userPositionsBackgroundHandler.StartLoopAsync(_backgroundHandlersCTS.Token);
        }
        if (!_mapObjectsBackgroundHandler.IsRunning)
        {
            _mapObjectsBackgroundHandler.ReloadSettings = true;
            _mapObjectsBackgroundHandler.MapObjectsChanged += OnMapObjectsChanged;
            _mapObjectsBackgroundHandler.ErrorOccured += OnBackgroundHandlerErrorOccured;
            _mapObjectsBackgroundTask = _mapObjectsBackgroundHandler.StartLoopAsync(_backgroundHandlersCTS.Token);
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

    private async Task LoadMapSettingsAsync(bool reloadMapStyle)
    {
        _coordinateSystem = _mapSettingsDataProvider.GetCurrentCoordinateSystem().Value;
        _krisMapType = _mapSettingsDataProvider.GetCurrentMapType().Value;

        if (reloadMapStyle || KrisMapStyle == null)
        {
            if (_krisMapType == KrisMapType.Military)
            {
                _mapTileRepository = _repositoryFactory.CreateMapTileRepository(_mapSettingsDataProvider.GetCurrentCustomMapTileSource());
                KrisMapStyle = await KrisMapStyleFactory.CreateStyleAsync(_krisMapType, _mapTileRepository.GetTile);
            }
            else
            {
                KrisMapStyle = await KrisMapStyleFactory.CreateStyleAsync(_krisMapType);
            }
        }

        CurrentPosition.CoordinateSystem = _coordinateSystem;
        OnPropertyChanged(nameof(CurrentPosition));
    }

    private async Task MoveToCurrentRegionAsync()
    {
        var query = new GetCurrentRegionQuery();
        var currentRegion = await MediatorSendAsync(query, CancellationToken.None);

        if (currentRegion != null)
        {
            MoveToRegion.Execute(currentRegion);
        }
    }

    private async Task MoveToCurrentPositionAsync()
    {
        var query = new GetCurrentPositionQuery();
        var currentPosition = await MediatorSendAsync(query, CancellationToken.None);

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
        var currentUser = await MediatorSendAsync(query, CancellationToken.None);
        if (currentUser == null || !currentUser.SessionId.HasValue || !currentUser.UserType.HasValue)
        {
            await _alertService.ShowToastAsync("Must join a session to create map objects");
            return;
        }

        var resultEventArgs = await _popupService.ShowPopupAsync<CreateMapPointPopupViewModel>(vm =>
        {
            vm.Setup(currentUser.Id, currentUser.Login, location);
        }) as ResultEventArgs<MapPointListModel>;
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
        if (pin.KrisType != KrisPinType.Point) return;

        var query = new GetCurrentUserQuery();
        var currentUser = await MediatorSendAsync(query, CancellationToken.None);
        if (currentUser == null || !currentUser.SessionId.HasValue || !currentUser.UserType.HasValue)
        {
            await _alertService.ShowToastAsync("Invalid user data");
            await LogoutUser();
        }

        var resultArgs = await _popupService.ShowPopupAsync<EditMapPointPopupViewModel>(async vm =>
        {
            vm.Setup(pin.KrisId, currentUser.Id, currentUser.Login, currentUser.UserType.Value);
            await vm.LoadMapPointDetailAsync();
        });
        if (resultArgs == null) return;

        if (resultArgs is LoadResultEventArgs<MapPointDetailModel> loadResult)
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
        else if (resultArgs is UpdateResultEventArgs<MapPointListModel> updateResult)
        {
            var result = updateResult.Result;
            if (result.IsFailed)
            {
                if (result.HasError<UnauthorizedError>())
                {
                    await _alertService.ShowToastAsync("Login expired");
                    await LogoutUser();
                }
                else if (result.HasError<EntityNotFoundError>())
                {
                    await _alertService.ShowToastAsync("Pin not found");
                }
                else
                {
                    await _alertService.ShowToastAsync(result.Errors.FirstMessage());
                }
            }
            else
            {
                await _alertService.ShowToastAsync("Map point updated");
                var pinToRemove = AllMapPins.FirstOrDefault(p => p.KrisPinType == KrisPinType.Point && p.Id == result.Value.Id);
                AllMapPins.Remove(pinToRemove);
                AllMapPins.Add(_krisMapObjectFactory.CreateMapPoint(result.Value));
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
                var pinToRemove = AllMapPins.FirstOrDefault(p => p.KrisPinType == KrisPinType.Point && p.Id == pin.KrisId);
                AllMapPins.Remove(pinToRemove);
            }
        }
    }

    private void AddCurrentUserPositionToMap(UserPositionModel userPosition)
    {
        CurrentPosition.Location = userPosition.Positions.First();
        OnPropertyChanged(nameof(CurrentPosition));

        var userPin = _krisMapObjectFactory.CreateUserPositionPin(userPosition, KrisPinType.Self);
        var oldUserPin = AllMapPins.FirstOrDefault(p => p.KrisPinType == KrisPinType.Self && p.Id == userPosition.UserId);
        AllMapPins.Remove(oldUserPin);
        AllMapPins.Add(userPin);
    }
    
    private void AddOtherUserPositionsToMap(IEnumerable<UserPositionModel> userPositions)
    {
        var userPins = userPositions.Select(p => _krisMapObjectFactory.CreateUserPositionPin(p, KrisPinType.User));
        UpdateMapPins(userPins);
    }

    private void AddMapObjectsToMap(IEnumerable<MapPointListModel> mapPoints, IEnumerable<Guid> deletedPoints)
    {
        var pointPins = mapPoints.Select(_krisMapObjectFactory.CreateMapPoint);
        UpdateMapPins(pointPins);
        foreach (var deletedPoint in deletedPoints)
        {
            var pin = AllMapPins.FirstOrDefault(pin => pin.Id == deletedPoint);
            if (pin != null) AllMapPins.Remove(pin);
        }
    }

    private async Task ShowMessageNotification(Guid id, string sender, string message)
    {
        if (Shell.Current.CurrentPage is ChatView) return;
        await _alertService.ShowNotificationAsync(id.GetHashCode(), sender, string.Empty, message);
    }

    // MISC
    private async void OnBackgroundHandlerErrorOccured(object sender, ResultEventArgs e)
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

    private async void OnLogout(object sender, LogoutMessage message)
    {
        _backgroundHandlersCTS.Cancel();

        try
        {
            await _userPositionsBackgroundTask;
        }
        catch (TaskCanceledException) { }
        finally
        {
            _userPositionsBackgroundHandler.UserPositionsChanged -= OnUserPositionsChanged;
            _userPositionsBackgroundHandler.ErrorOccured -= OnBackgroundHandlerErrorOccured;
        }

        try
        {
            await _mapObjectsBackgroundTask;
        }
        catch (TaskCanceledException) { }
        finally
        {
            _mapObjectsBackgroundHandler.MapObjectsChanged -= OnMapObjectsChanged;
            _mapObjectsBackgroundHandler.ErrorOccured -= OnBackgroundHandlerErrorOccured;
        }

        _currentPositionBackgroundHandler.CurrentPositionChanged -= OnCurrentPositionChanged;
        _currentPositionBackgroundHandler.ErrorOccured -= OnBackgroundHandlerErrorOccured;
        CurrentPositionBackgroundService.StopService();
        _currentPositionBackgroundHandler.IsRunning = false;

        _backgroundHandlersCTS.Dispose();
        _backgroundHandlersCTS = null;

        _messageReceiver.MessageReceived -= OnMessageReceived;
        await _messageReceiver.Disconnect();

        KrisMapStyle = null;
        _mapTileRepository?.Dispose();

        AllMapPins.Clear();
        OnPropertyChanged(nameof(AllMapPins));
    }

    private async void OnBackgroundContextChanged(object sender, MessageBase message)
    {
        _currentPositionBackgroundHandler.ReloadSettings = true;
        _userPositionsBackgroundHandler.ReloadSettings = true;
        _mapObjectsBackgroundHandler.ReloadSettings = true;

        if (message is CurrentSessionChangedMessage)
        {
            AllMapPins.Clear();
            OnPropertyChanged(nameof(AllMapPins));

            await _messageReceiver.Disconnect();
            await _messageReceiver.Connect();
        }
    }

    private void UpdateMapPins(IEnumerable<KrisMapPinViewModel> newPins)
    {
        foreach (var pin in newPins)
        {
            var oldPin = AllMapPins.FirstOrDefault(p => p.Id == pin.Id && p.KrisPinType == pin.KrisPinType);
            if (oldPin == null)
            {
                AllMapPins.Add(pin);
            }
            else
            {
                if (oldPin.TimeStamp < pin.TimeStamp)
                {
                    AllMapPins.Remove(oldPin);
                    AllMapPins.Add(pin);
                }
            }
        }
    }
}
