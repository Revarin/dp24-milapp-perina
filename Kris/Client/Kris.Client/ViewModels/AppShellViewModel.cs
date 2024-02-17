using Kris.Client.Core.Services;

namespace Kris.Client.ViewModels;

public sealed class AppShellViewModel : ViewModelBase
{
    public AppShellViewModel(IRouterService navigationService, IAlertService alertService)
        : base(navigationService, alertService)
    {
    }
}
