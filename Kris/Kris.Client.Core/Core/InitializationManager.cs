using Kris.Client.Common;

namespace Kris.Client.Core
{
    public class InitializationManager : IInitializationManager
    {
        private readonly INavigationService _navigationService;
        private readonly IPermissionsService _permissionsService;
        private readonly IAlertService _alertService;
        private readonly IPreferencesStore _preferencesStore;

        public InitializationManager(INavigationService navigationService, IPermissionsService permissionsService, IAlertService alertService, IPreferencesStore preferencesStore)
        {
            _navigationService = navigationService;
            _permissionsService = permissionsService;
            _alertService = alertService;
            _preferencesStore = preferencesStore;
        }

        public void InitializeNavigation(IEnumerable<Type> pages)
        {
            _navigationService.RegisterRoutes(pages);
        }

        public async Task InitializePermissionsAsync()
        {
            // GPS permission when in use
            var permissionsStatus = await _permissionsService.CheckAndRequestPermissionAsync<Permissions.LocationWhenInUse>();
            if (!permissionsStatus.HasFlag(PermissionStatus.Granted))
            {
                await _alertService.ShowAlertAsync(I18n.Keys.MapLocationPermissionDeniedTitle, I18n.Keys.MapLocationPermissionDeniedMessage);
            }

            // TODO: GPS permission when not in use
        }

        public async Task InitialiteUserDataAsync()
        {
            var userSettings = _preferencesStore.GetUserSettings();

            if (userSettings.UserId < 0 || string.IsNullOrEmpty(userSettings.UserName))
            {
                // TODO: Show popup with textbox for username
            }
        }
    }
}
