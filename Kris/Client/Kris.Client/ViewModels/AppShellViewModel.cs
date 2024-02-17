using Kris.Client.Core.Services;
using MediatR;

namespace Kris.Client.ViewModels;

public sealed class AppShellViewModel : ViewModelBase
{
    public AppShellViewModel(IMediator mediator, IRouterService navigationService, IAlertService alertService)
        : base(mediator, navigationService, alertService)
    {
    }
}
