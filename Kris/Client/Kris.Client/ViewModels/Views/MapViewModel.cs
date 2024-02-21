using CommunityToolkit.Mvvm.Input;
using Kris.Client.Common.Utility;
using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using Kris.Client.Views;
using MediatR;

namespace Kris.Client.ViewModels.Views;

public sealed partial class MapViewModel : PageViewModelBase
{
    public MapViewModel(IMediator mediator, IRouterService navigationService, IAlertService alertService)
        : base(mediator, navigationService, alertService)
    {
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
