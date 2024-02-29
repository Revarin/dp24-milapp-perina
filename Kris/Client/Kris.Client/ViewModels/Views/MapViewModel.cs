using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentResults;
using Kris.Client.Common.Enums;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Events;
using Kris.Client.Components.Map;
using Kris.Client.Core.Listeners;
using Kris.Client.Core.Listeners.Events;
using Kris.Client.Core.Messages;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Utility;
using Kris.Client.ViewModels.Popups;
using Kris.Common.Extensions;
using MediatR;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;

namespace Kris.Client.ViewModels.Views;

public sealed partial class MapViewModel : PageViewModelBase
{
    private readonly IPopupService _popupService;
    private readonly ICurrentPositionListener _selfPositionListener;
    private readonly IUserPositionsListener _othersPositionListener;

    [ObservableProperty]
    private MapSpan _currentRegion;
    [ObservableProperty]
    private MoveToRegionRequest _moveToRegion = new MoveToRegionRequest();

    [ObservableProperty]
    private ObservableCollection<MapPin> _allMapPins = new ObservableCollection<MapPin>();
    [ObservableProperty]
    private ObservableCollection<UserPositionModel> _userPositions = new ObservableCollection<UserPositionModel>();

    private CancellationTokenSource _selfPositionCTS;
    private Task _selfPositionTask;
    private CancellationTokenSource _othersPositionCTS;
    private Task _othersPositionTask;

    public MapViewModel(IPopupService popupService, ICurrentPositionListener currentPositionListener, IUserPositionsListener userPositionsListener,
        IMediator mediator, IRouterService navigationService, IMessageService messageService, IAlertService alertService)
        : base(mediator, navigationService, messageService, alertService)
    {
        _popupService = popupService;
        _selfPositionListener = currentPositionListener;
        _othersPositionListener = userPositionsListener;

        _messageService.Register<LogoutMessage>(this, OnLogout);
        _messageService.Register<CurrentSessionChangedMessage>(this, RestartPositionListeners);
        _messageService.Register<ConnectionSettingsChangedMessage>(this, RestartPositionListeners);
    }

    [RelayCommand]
    private void OnAppearing()
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
    }

    [RelayCommand]
    private async Task OnMapLoaded()
    {
        var query = new GetCurrentRegionQuery();
        var currentRegion = await _mediator.Send(query, CancellationToken.None);

        if (currentRegion != null)
        {
            MoveToRegion.Execute(currentRegion);
        }
    }

    [RelayCommand]
    private async Task OnCurrentPositionClicked()
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

    [RelayCommand]
    private async Task OnMapClicked(MapClickedEventArgs e)
    {
        // Map point creation
        var query = new GetCurrentUserQuery();
        var currentUser = await _mediator.Send(query, CancellationToken.None);
        if (currentUser == null || !currentUser.SessionId.HasValue || !currentUser.UserType.HasValue)
        {
            await _alertService.ShowToastAsync("Must join a session to create map objects");
            return;
        }

        var resultEventArgs = await _popupService.ShowPopupAsync<CreateMapPointPopupViewModel>(vm =>
        {
            vm.Location = e.Location;
        }) as ResultEventArgs<Guid>;
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
        }
    }

    [RelayCommand]
    private async Task OnLogoutClicked()
    {
        await LogoutUser();
    }

    private void OnSelfPositionPositionChanged(object sender, LocationEventArgs e)
    {
        var userPin = new MapPin
        {
            Id = e.UserId,
            Name = e.UserName,
            Updated = DateTime.Now,
            Location = e.Location,
            PinType = KrisPinType.Self,
            ImageSource = ImageSource.FromFile("point_green.png")
        };

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
        UserPositions = e.Positions.UnionBy(UserPositions, position => position.UserId).ToObservableCollection();
        var userPins = UserPositions.Select(position => new MapPin(position));
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

        UserPositions.Clear();
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

        if (message is CurrentSessionChangedMessage)
        {
            UserPositions.Clear();
            AllMapPins.Clear();
        }
    }
}
