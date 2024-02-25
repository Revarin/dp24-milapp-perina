using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Events;
using Kris.Client.Core.Listeners;
using Kris.Client.Core.Listeners.Events;
using Kris.Client.Core.Messages;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Utility;
using Kris.Common.Extensions;
using MediatR;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;

namespace Kris.Client.ViewModels.Views;

public sealed partial class MapViewModel : PageViewModelBase
{
    private readonly ICurrentPositionListener _selfPositionListener;
    private readonly IUserPositionsListener _othersPositionListener;

    [ObservableProperty]
    private MapSpan _currentRegion;
    [ObservableProperty]
    private MoveToRegionRequest _moveToRegion = new MoveToRegionRequest();
    [ObservableProperty]
    private ObservableCollection<UserPositionModel> _userPositions = new ObservableCollection<UserPositionModel>();

    private CancellationTokenSource _selfPositionCTS;
    private Task _selfPositionTask;
    private CancellationTokenSource _othersPositionCTS;
    private Task _othersPositionTask;

    public MapViewModel(ICurrentPositionListener currentPositionListener, IUserPositionsListener userPositionsListener,
        IMediator mediator, IRouterService navigationService, IMessageService messageService, IAlertService alertService)
        : base(mediator, navigationService, messageService, alertService)
    {
        _selfPositionListener = currentPositionListener;
        _othersPositionListener = userPositionsListener;

        _messageService.Register<LogoutMessage>(this, OnLogout);
        _messageService.Register<RestartPositionListenersMessage>(this, OnRestartPositionListeners);
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
    private async Task OnLogoutClicked()
    {
        await LogoutUser();
    }

    private async void OnSelfPositionPositionChanged(object sender, LocationEventArgs e)
    {
        // TODO: Update user point (GUI only)
        await _alertService.ShowToastAsync(e.Location.ToString());
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

    private async void OnOthersPositionPositionChanged(object sender, UserPositionsEventArgs e)
    {
        // TODO: Update user point (GUI only)
        await _alertService.ShowToastAsync("XXX");
        UserPositions = e.Positions.UnionBy(UserPositions, position => position.UserId).ToObservableCollection();
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
    }

    private async void OnRestartPositionListeners(object sender, RestartPositionListenersMessage message)
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
    }
}
