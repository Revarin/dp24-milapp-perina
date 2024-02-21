using Kris.Client.Core.Services;
using Kris.Client.Views;
using MediatR;

namespace Kris.Client.ViewModels.Views;

public sealed class AppShellViewModel : PageViewModelBase
{
    public AppShellViewModel(IMediator mediator, IRouterService navigationService, IAlertService alertService)
        : base(mediator, navigationService, alertService)
    {
        Routing.RegisterRoute(nameof(RegisterView), typeof(RegisterView));
        Routing.RegisterRoute(nameof(SessionSettingsView), typeof(SessionSettingsView));
    }
}
