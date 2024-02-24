using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Utility;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Utility;
using Kris.Client.Views;
using MediatR;
using Microsoft.Maui.Maps;

namespace Kris.Client.ViewModels.Views;

public sealed partial class MapViewModel : PageViewModelBase
{
    [ObservableProperty]
    private MapSpan _currentRegion;
    [ObservableProperty]
    private MoveToRegionRequest _moveToRegion = new MoveToRegionRequest();

    public MapViewModel(IMediator mediator, IRouterService navigationService, IAlertService alertService)
        : base(mediator, navigationService, alertService)
    {
    }

    [RelayCommand]
    private async Task OnMapLoaded()
    {
        var query = new GetCurrentRegionQuery();
        var currentRegion = await _mediator.Send(query, CancellationToken.None);

        await _alertService.ShowToastAsync(currentRegion.Center.ToString());
        if (currentRegion != null)
        {
            MoveToRegion.Execute(currentRegion);
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
}
