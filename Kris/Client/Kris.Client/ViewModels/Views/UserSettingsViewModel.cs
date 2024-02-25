using Kris.Client.Core.Services;
using MediatR;

namespace Kris.Client.ViewModels.Views;

public sealed partial class UserSettingsViewModel : PageViewModelBase
{
    public UserSettingsViewModel(IMediator mediator, IRouterService navigationService, IMessageService messageService, IAlertService alertService)
        : base(mediator, navigationService, messageService, alertService)
    {
    }
}
