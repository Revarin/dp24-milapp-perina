using System.Windows.Input;
using System.Collections.ObjectModel;
using Microsoft.Maui.Maps;
using Microsoft.Extensions.Configuration;
using Kris.Client.Behaviors;
using Kris.Client.Data;
using Kris.Client.Core;
using Kris.Client.Common;

namespace Kris.Client.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private readonly AppSettings _settings;
        private readonly IAlertService _alertService;
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

        public MapViewModel(IAlertService alertService, IMessageService messageService, IPermissionsService permissionsService,
            IPreferencesStore preferencesStore, IGpsService gpsService, ILocationFacade locationFacade, IConfiguration config)
        {
            _alertService = alertService;
            _messageService = messageService;
            _permissionsService = permissionsService;
            _preferencesStore = preferencesStore;
            _gpsService = gpsService;
            _locationFacade = locationFacade;
            _settings = config.GetRequiredSection("Settings").Get<AppSettings>();

            CurrentRegion = new MapSpan(new Location(), 10, 10);
            LoadedCommand = new Command(OnLoaded);
            MoveToUserCommand = new Command(OnMoveToUser);

            _messageService.Register<ShellInitializedMessage>(this, OnShellInitializedAsync);
            _messageService.Register<ConnectionSettingsChangedMessage>(this, OnConnectionSettingsChanged);
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
            _connectionSettings = _preferencesStore.GetConnectionSettings();

            var locationPermission = await _permissionsService.CheckPermissionAsync<Permissions.LocationWhenInUse>();
            if (locationPermission.HasFlag(PermissionStatus.Granted) && await _gpsService.IsGpsEnabledAsync(2))
            {
                _gpsService.RaiseGpsLocationEvent += OnGpsNewLocation;
                if (!_gpsService.IsListening)
                {
                    _gpsService.StartListening(_connectionSettings.GpsInterval, _connectionSettings.GpsInterval);
                }
            }
            else
            {
                _alertService.ShowToast($"GPS service is not available.");
            }

            if (_connectionSettings.UserId > 0 && _settings.ServerEnabled)
            {
                _locationFacade.RaiseUserLocationsEvent += OnLoadUsersLocations;
                if (!_locationFacade.IsListening)
                {
                    _locationFacade.StartListeningToUsersLocations(_connectionSettings.UserId, _connectionSettings.UsersLocationInterval);
                }
            }
            else
            {
                _alertService.ShowToast($"Cannot load users locations.");
            }
        }

        private void OnConnectionSettingsChanged(object sender, ConnectionSettingsChangedMessage message)
        {
            _connectionSettings = message.Settings;

            if (message.GpsIntervalChanged)
            {
                _gpsService.StopListening();
                _gpsService.StartListening(_connectionSettings.GpsInterval, _connectionSettings.GpsInterval);
            }
            if (message.UsersLocationIntervalChanged && _settings.ServerEnabled)
            {
                _locationFacade.StopListeningToUsersLocation();
                _locationFacade.StartListeningToUsersLocations(_connectionSettings.UserId, _connectionSettings.UsersLocationInterval);
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

        private async void OnGpsNewLocation(object sender, GpsLocationEventArgs e)
        {
#if DEBUG
            _alertService.ShowToast($"LAT:{e.Location.Latitude} LONG:{e.Location.Longitude} ALT:{e.Location.Altitude}");
#endif
            Task SaveUserLocationTask = null;
            if (_connectionSettings.UserId > 0)
            {
                SaveUserLocationTask = _locationFacade.SaveUserLocationAsync(_connectionSettings.UserId, e.Location);
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

            if (SaveUserLocationTask != null) await SaveUserLocationTask;
        }

        private void OnLoadUsersLocations(object sender, UsersLocationsEventArgs e)
        {
#if DEBUG
            _alertService.ShowToast($"User locations loaded");
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
