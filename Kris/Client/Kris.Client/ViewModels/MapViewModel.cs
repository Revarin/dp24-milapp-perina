using Kris.Client.Core.Services;
using MediatR;

namespace Kris.Client.ViewModels;

public sealed partial class MapViewModel : ViewModelBase
{
    public MapViewModel(IMediator mediator, IRouterService navigationService, IAlertService alertService)
        : base(mediator, navigationService, alertService)
    {
    }
}
