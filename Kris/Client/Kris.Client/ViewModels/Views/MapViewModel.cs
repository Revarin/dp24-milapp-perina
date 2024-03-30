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
using Kris.Client.Core.Listeners;
using Kris.Client.Core.Listeners.Events;
using Kris.Client.Core.Messages;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Data.Database;
using Kris.Client.Data.Providers;
using Kris.Client.Utility;
using Kris.Client.ViewModels.Items;
using Kris.Client.ViewModels.Popups;
using Kris.Client.ViewModels.Utility;
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
    private readonly IBackgroundLoop _backgroundLoop;
    private readonly ICurrentPositionBackgroundHandler _currentPositionBackgroundHandler;
    private readonly IUserPositionsBackgroundHandler _userPositionsBackgroundHandler;
    private readonly IMapObjectsBackgroundHandler _mapObjectsBackgroundHandler;
    private readonly IMessageReceiver _messageReceiver;

    [ObservableProperty]
    private MapSpan _currentRegion;
    [ObservableProperty]
    private MoveToRegionRequest _moveToRegion = new MoveToRegionRequest();
    [ObservableProperty]
    private LocationCoordinates _currentPosition = new LocationCoordinates();

    [ObservableProperty]
    private KrisMapStyle _krisMapStyle = null;
    [ObservableProperty]
    private ObservableCollection<KrisMapPinViewModel> _allMapPins = new ObservableCollection<KrisMapPinViewModel>();

    private CancellationTokenSource _backgroundLoopCTS;
    private Task _backgroundLoopTask;
    private IMapTileRepository _mapTileRepository;

    private CoordinateSystem _coordinateSystem;
    private KrisMapType _krisMapType;

    public MapViewModel(IMapSettingsDataProvider mapSettingsDataProvider, IRepositoryFactory repositoryFactory,
        IKrisMapObjectFactory krisMapObjectFactory, IBackgroundLoop backgroundLoop,
        ICurrentPositionBackgroundHandler currentPositionBackgroundHandler, IUserPositionsBackgroundHandler userPositionsBackgroundHandler,
        IMapObjectsBackgroundHandler mapObjectsBackgroundHandler, IMessageReceiver messageReceiver,
        IMediator mediator, IRouterService navigationService, IMessageService messageService, IPopupService popupService, IAlertService alertService)
        : base(mediator, navigationService, messageService, popupService, alertService)
    {
        _mapSettingsDataProvider = mapSettingsDataProvider;
        _repositoryFactory = repositoryFactory;
        _krisMapObjectFactory = krisMapObjectFactory;
        _backgroundLoop = backgroundLoop;
        _currentPositionBackgroundHandler = currentPositionBackgroundHandler;
        _userPositionsBackgroundHandler = userPositionsBackgroundHandler;
        _mapObjectsBackgroundHandler = mapObjectsBackgroundHandler;
        _messageReceiver = messageReceiver;

        _messageService.Register<LogoutMessage>(this, OnLogout);
        _messageService.Register<CurrentSessionChangedMessage>(this, OnBackgroundContextChanged);
        _messageService.Register<ConnectionSettingsChangedMessage>(this, OnBackgroundContextChanged);
        _messageService.Register<MapSettingsChangedMessage>(this, async (sender, msg) => await LoadMapSettingsAsync(true));
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
    private async Task OnLogoutButtonClicked()
    {
        var confirmation = await _popupService.ShowPopupAsync<ConfirmationPopupViewModel>(vm => vm.Message = "Do you want to logout?") as ConfirmationEventArgs;
        if (confirmation == null || !confirmation.IsConfirmed) return;
        await LogoutUser();
    }
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
    private void OnMapObjectsChanged(object sender, MapObjectsEventArgs e) => AddMapObjectsToMap(e.MapPoints);
    private async void OnMessageReceived(object sender, MessageReceivedEventArgs e) => await ShowMessageNotification(e.SenderName, e.Body);

    // CORE
    private void StartBackgroudListeners()
    {
        if (!_backgroundLoop.IsRunning)
        {
            _currentPositionBackgroundHandler.CurrentPositionChanged += OnCurrentPositionChanged;
            _userPositionsBackgroundHandler.UserPositionsChanged += OnUserPositionsChanged;
            _mapObjectsBackgroundHandler.MapObjectsChanged += OnMapObjectsChanged;
            _currentPositionBackgroundHandler.ErrorOccured += OnBackgroundHandlerErrorOccured;
            _userPositionsBackgroundHandler.ErrorOccured += OnBackgroundHandlerErrorOccured;
            _mapObjectsBackgroundHandler.ErrorOccured += OnBackgroundHandlerErrorOccured;

            _backgroundLoop.RegisterHandler(_currentPositionBackgroundHandler);
            _backgroundLoop.RegisterHandler(_userPositionsBackgroundHandler);
            _backgroundLoop.RegisterHandler(_mapObjectsBackgroundHandler);

            _backgroundLoopCTS = new CancellationTokenSource();
            _backgroundLoopTask = _backgroundLoop.Start(_backgroundLoopCTS.Token);
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
        AllMapPins = userPins.UnionBy(AllMapPins, pin => new { pin.Id, pin.KrisPinType }).ToObservableCollection();
    }

    private void AddMapObjectsToMap(IEnumerable<MapPointListModel> mapPoints)
    {
        var pointPins = mapPoints.Select(_krisMapObjectFactory.CreateMapPoint);
        AllMapPins = pointPins.UnionBy(AllMapPins, pin => new { pin.Id, pin.KrisPinType }).ToObservableCollection();
    }

    private async Task ShowMessageNotification(string sender, string message)
    {
        // TODO: Better notification
        if (Shell.Current.CurrentPage is ChatView) return;
        await _alertService.ShowToastAsync($"{sender}: {message}");
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
        if (_backgroundLoop.IsRunning)
        {
            _backgroundLoopCTS.Cancel();

            try
            {
                await _backgroundLoopTask;
            }
            catch (TaskCanceledException) { }
            finally
            {
                _currentPositionBackgroundHandler.CurrentPositionChanged -= OnCurrentPositionChanged;
                _userPositionsBackgroundHandler.UserPositionsChanged -= OnUserPositionsChanged;
                _mapObjectsBackgroundHandler.MapObjectsChanged -= OnMapObjectsChanged;
                _currentPositionBackgroundHandler.ErrorOccured -= OnBackgroundHandlerErrorOccured;
                _userPositionsBackgroundHandler.ErrorOccured -= OnBackgroundHandlerErrorOccured;
                _mapObjectsBackgroundHandler.ErrorOccured -= OnBackgroundHandlerErrorOccured;

                _backgroundLoop.ClearHandlers();
                _backgroundLoopCTS.Dispose();
            }
        }

        if (_messageReceiver.IsConnected)
        {
            _messageReceiver.MessageReceived -= OnMessageReceived;
            await _messageReceiver.Disconnect();
        }

        KrisMapStyle = null;
        _mapTileRepository?.Dispose();

        AllMapPins.Clear();
    }

    private async void OnBackgroundContextChanged(object sender, MessageBase message)
    {
        // TODO: Rename a move
        _backgroundLoop.ReloadSettings = true;

        if (message is CurrentSessionChangedMessage)
        {
            AllMapPins.Clear();
            _backgroundLoop.ResetHandlers();

            await _messageReceiver.Disconnect();
            await _messageReceiver.Connect();
        }
    }
}
