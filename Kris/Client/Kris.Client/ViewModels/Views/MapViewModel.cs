using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Events;
using Kris.Client.Common.Utility;
using Kris.Client.Core.Listeners;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Utility;
using Kris.Client.Views;
using MediatR;
using Microsoft.Maui.Maps;

namespace Kris.Client.ViewModels.Views;

public sealed partial class MapViewModel : PageViewModelBase
{
    private readonly ICurrentPositionListener _currentPositionListener;

    [ObservableProperty]
    private MapSpan _currentRegion;
    [ObservableProperty]
    private MoveToRegionRequest _moveToRegion = new MoveToRegionRequest();

    private CancellationTokenSource _currentPositionCTS;

    public MapViewModel(ICurrentPositionListener currentPositionListener,
        IMediator mediator, IRouterService navigationService, IAlertService alertService)
        : base(mediator, navigationService, alertService)
    {
        _currentPositionListener = currentPositionListener;

        _currentPositionCTS = new CancellationTokenSource();
    }

    protected override Task InitAsync()
    {
        _currentPositionListener.PositionChanged += OnPositionChanged;
        _currentPositionListener.ErrorOccured += OnPositionListenerErrorOccured;
        if (!_currentPositionListener.IsListening)
        {
            _currentPositionListener.StartListening(_currentPositionCTS.Token);
        }

        return base.InitAsync();
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
        var ct = new CancellationToken();
        var command = new LogoutUserCommand();
        await _mediator.Send(command, ct);
        await _navigationService.GoToAsync(nameof(LoginView), RouterNavigationType.ReplaceUpward);
    }

    private async void OnPositionChanged(object sender, LocationEventArgs e)
    {
        await _alertService.ShowToastAsync(e.Location.ToString());
    }

    private async void OnPositionListenerErrorOccured(object sender, ResultEventArgs e)
    {
        if (e.Result.HasError<ServiceDisabledError>())
        {
            await _alertService.ShowToastAsync("GPS service is disabled", ToastDuration.Long);
        }
        else if (e.Result.HasError<ServicePermissionError>())
        {
            await _alertService.ShowToastAsync("GPS service is not permitted", ToastDuration.Long);
        }
        else
        {
            await _alertService.ShowToastAsync("Unknown error when listening to position", ToastDuration.Long);
        }
    }
}
