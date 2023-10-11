using System.Windows.Input;
using System.Collections.ObjectModel;
using Microsoft.Maui.Maps;
using CommunityToolkit.Maui.Alerts;
using Kris.Client.Behaviors;
using Kris.Client.Data;
using Kris.Client.Core;

namespace Kris.Client.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService;
        private readonly IPermissionsService _permissionsService;
        private readonly IPreferencesStore _preferencesStore;
        private readonly IGpsService _gpsService;
        private readonly ILocationFacade _locationFacade;

        private MapSpan _currentRegion;
        public MapSpan CurrentRegion
        {
            get { return _currentRegion; }
            set { SetPropertyValue(ref _currentRegion, value); }
        }
        public ObservableCollection<MapPin> PinsSource { get; } = new();
        public MoveToRegionRequest MoveToRegion { get; init; } = new MoveToRegionRequest();

        public ICommand LoadedCommand { get; init; }
        public ICommand MoveToUserCommand { get; init; }

        private ConnectionSettings _connectionSettings;

        public MapViewModel(IMessageService messageService, IPermissionsService permissionsService,
            IPreferencesStore preferencesStore, IGpsService gpsService, ILocationFacade locationFacade)
        {
            _messageService = messageService;
            _permissionsService = permissionsService;
            _preferencesStore = preferencesStore;
            _gpsService = gpsService;
            _locationFacade = locationFacade;

            CurrentRegion = new MapSpan(new Location(), 10, 10);
            LoadedCommand = new Command(OnLoaded);
            MoveToUserCommand = new Command(OnMoveToUser);

            _messageService.Register<ShellInitializedMessage>(this, OnShellInitializedAsync);
            _messageService.Register<ConnectionSettingsChangedMessage>(this, OnConnectionSettingsChangedAsync);

            _connectionSettings = _preferencesStore.GetConnectionSettings();
        }

        private void OnLoaded()
        {
            var lastRegion = _preferencesStore.GetLastRegion();
            if (lastRegion != null)
            {
                MoveToRegion.Execute(lastRegion);
            }
        }

        private async void OnShellInitializedAsync(object sender, ShellInitializedMessage message)
        {
            var locationPermission = await _permissionsService.CheckPermissionAsync<Permissions.LocationWhenInUse>();
            if (locationPermission.HasFlag(PermissionStatus.Granted) && await _gpsService.IsGpsEnabled(2))
            {
                _gpsService.RaiseGpsLocationEvent += OnGpsNewLocationAsync;
                if (!_gpsService.IsListening)
                {
                    _gpsService.StartListeningAsync(_connectionSettings.GpsInterval, _connectionSettings.GpsInterval);
                }
            }
            else
            {
                var gpsUnavailableToast = Toast.Make($"GPS service is not available.");
                await gpsUnavailableToast.Show();
            }

            if (_connectionSettings.UserId > 0)
            {
                _locationFacade.RaiseUserLocationsEvent += OnLoadUsersLocationsAsync;
                if (!_locationFacade.IsListening)
                {
                    _locationFacade.StartListeningToUserLocationsAsync(_connectionSettings.UserId, _connectionSettings.UsersLocationInterval);
                }
            }
            else
            {
                var usersLocationsUnavailableToast = Toast.Make($"Cannot load users locations.");
                await usersLocationsUnavailableToast.Show();
            }
        }

        private async void OnConnectionSettingsChangedAsync(object sender, ConnectionSettingsChangedMessage message)
        {
            _connectionSettings = message.Settings;

            if (message.GpsIntervalChanged)
            {
                await _gpsService.StopListening();
                _gpsService.StartListeningAsync(_connectionSettings.GpsInterval, _connectionSettings.GpsInterval);
            }
            if (message.UsersLocationIntervalChanged)
            {
                await _locationFacade.StopListening();
                _locationFacade.StartListeningToUserLocationsAsync(_connectionSettings.UserId, _connectionSettings.UsersLocationInterval);
            }
        }

        private void OnMoveToUser()
        {
            var user = PinsSource.SingleOrDefault(p => p.PinType == CustomPinType.Myself);
            if (user != null)
            {
                var userRegion = MapSpan.FromCenterAndRadius(user.Location, CurrentRegion.Radius);
                MoveToRegion.Execute(userRegion);
            }
        }

        private async void OnGpsNewLocationAsync(object sender, GpsLocationEventArgs e)
        {
#if DEBUG
            var t = Toast.Make($"LAT:{e.Location.Latitude} LONG:{e.Location.Longitude} ALT:{e.Location.Altitude}");
            await t.Show();
#endif
            if (_connectionSettings.UserId > 0)
            {
                await _locationFacade.SaveUserLocationAsync(_connectionSettings.UserId, e.Location);
            }

            var user = PinsSource.SingleOrDefault(p => p.PinType == CustomPinType.Myself);
            if (user != null)
            {
                PinsSource.Remove(user);
            }
            PinsSource.Add(new MapPin
            {
                Location = e.Location,
                PinType = CustomPinType.Myself,
                ImageSource = ImageSource.FromFile("myself.png"),
                Name = "Myself",
            });
        }

        private async void OnLoadUsersLocationsAsync(object sender, UsersLocationsEventArgs e)
        {
#if DEBUG
            var t = Toast.Make($"User locations loaded");
            await t.Show();
#endif
            foreach (var location in e.UserLocations)
            {
                var pin = PinsSource.SingleOrDefault(p => p.Id == location.UserId);
                if (pin != null)
                {
                    PinsSource.Remove(pin);
                }

                PinsSource.Add(new MapPin
                {
                    Location = location.Location,
                    PinType = CustomPinType.Users,
                    ImageSource = ImageSource.FromFile("users.png"),
                    Name = location.UserName,
                    Id = location.UserId.Value
                });
            }
        }
    }
}
