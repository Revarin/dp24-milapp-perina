using Kris.Client.Core.Services;
using Kris.Client.Data.Providers;
using MediatR;

namespace Kris.Client.ViewModels.Views;

public sealed partial class UserSettingsViewModel : PageViewModelBase
{
    private readonly IUserSettingsDataProvider _settingsDataProvider;

    public UserSettingsViewModel(IUserSettingsDataProvider settingsDataProvider,
        IMediator mediator, IRouterService navigationService, IMessageService messageService, IAlertService alertService)
        : base(mediator, navigationService, messageService, alertService)
    {
        _settingsDataProvider = settingsDataProvider;
    }
}
