using Kris.Client.Core.Services;
using MediatR;

namespace Kris.Client.ViewModels;

public sealed partial class MenuViewModel : ViewModelBase
{
    public MenuViewModel(IMediator mediator, IRouterService navigationService, IAlertService alertService)
        : base(mediator, navigationService, alertService)
    {
    }
}
