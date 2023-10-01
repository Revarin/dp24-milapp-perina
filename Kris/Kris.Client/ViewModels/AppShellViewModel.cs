using System.Windows.Input;
using Kris.Client.Core;
using Kris.Client.Common;

namespace Kris.Client.ViewModels
{
    public class AppShellViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService;
        private readonly INavigationService _navigationService;
        private readonly IPermissionsService _permissionsService;
        private readonly IAlertService _alertService;
        private readonly IPreferencesStore _preferencesStore;

        public ICommand AppearingCommand { get; init; }

        private List<Type> _contentPages = new()
        {
            typeof(MapView),
            typeof(MenuView),
            typeof(ConnectionSettingsView),
            typeof(TestView)
        };

        public AppShellViewModel(IMessageService messageService, INavigationService navigationService,
            IPermissionsService permissionsService, IAlertService alertService, IPreferencesStore preferencesStore)
        {
            _messageService = messageService;
            _navigationService = navigationService;
            _permissionsService = permissionsService;
            _alertService = alertService;
            _preferencesStore = preferencesStore;

            AppearingCommand = new Command(OnAppearing);
        }

        private async void OnAppearing()
        {
            _navigationService.RegisterRoutes(_contentPages);

            await AskForPermissions();
            await LoadUserData();

            _messageService.Send(new ShellInitializedMessage());
        }

        private async Task AskForPermissions()
        {
            // GPS permission
            var permissionsStatus = await _permissionsService.CheckAndRequestPermissionAsync<Permissions.LocationWhenInUse>();
            if (!permissionsStatus.HasFlag(PermissionStatus.Granted))
            {
                await _alertService.ShowAlertAsync(I18n.Keys.MapLocationPermissionDeniedTitle, I18n.Keys.MapLocationPermissionDeniedMessage);
            }
            // TODO: GPS when not in use
        }

        private async Task LoadUserData()
        {
            var connectionSettings = _preferencesStore.GetConnectionSettings();

            if (connectionSettings.UserId < 0 || string.IsNullOrEmpty(connectionSettings.UserName))
            {
                var newUserName = await _alertService.ShowPromptAsync(I18n.Keys.NewUserNamePromptTitle, I18n.Keys.NewUserNamePromptMessage, cancel: null);

                // TODO: Send to server

                _preferencesStore.SetConnectionSettings(new ConnectionSettings
                {
                    UserId = 0,
                    UserName = newUserName,
                });
            }
        }
    }
}
