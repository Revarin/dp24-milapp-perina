using System.Windows.Input;
using Kris.Client.Common;

namespace Kris.Client.ViewModels
{
    public class AppShellViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPermissionsService _permissionsService;
        private readonly IAlertService _alertService;

        public ICommand AppearingCommand { get; init; }

        private List<Type> _contentPages = new List<Type>()
        {
            typeof(MapView),
            typeof(MenuView),
            typeof(ConnectionSettingsView),
            typeof(TestView)
        };

        public AppShellViewModel(INavigationService navigationService, IPermissionsService permissionsService, IAlertService alertService)
        {
            _navigationService = navigationService;
            _permissionsService = permissionsService;
            _alertService = alertService;

            AppearingCommand = new Command(OnAppearing);
        }

        private async void OnAppearing()
        {
            _navigationService.RegisterRoutes(_contentPages);

            // GPS permission
            var permissionsStatus = await _permissionsService.CheckAndRequestPermissionAsync<Permissions.LocationWhenInUse>();
            if (!permissionsStatus.HasFlag(PermissionStatus.Granted))
            {
                await _alertService.ShowAlertAsync(I18n.Keys.MapLocationPermissionDeniedTitle, I18n.Keys.MapLocationPermissionDeniedMessage);
            }
        }
    }
}
