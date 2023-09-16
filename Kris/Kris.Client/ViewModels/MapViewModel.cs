using System.Windows.Input;
using Microsoft.Maui.Maps;
using Kris.Client.Behaviors;
using Kris.Client.Common;
using CommunityToolkit.Maui.Alerts;

namespace Kris.Client.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IPreferencesStore _preferencesStore;
        private readonly IGpsService _gpsService;

        private bool _showingUserLocation;
        public bool ShowingUserLocation
        {
            get { return _showingUserLocation; }
            set { SetPropertyValue(ref _showingUserLocation, value); }
        }
        private MapSpan _currentRegion;
        public MapSpan CurrentRegion
        {
            get { return _currentRegion; }
            set { SetPropertyValue(ref _currentRegion, value); }
        }
        public MoveToRegionRequest MoveToRegion { get; init; } = new MoveToRegionRequest();

        public ICommand LoadedCommand { get; init; }
        public ICommand TestCommand { get; init; }

        public MapViewModel(IPermissionsService permissionsService, IPreferencesStore preferencesStore, IGpsService gpsService)
        {
            _permissionsService = permissionsService;
            _preferencesStore = preferencesStore;
            _gpsService = gpsService;

            ShowingUserLocation = false;
            CurrentRegion = new MapSpan(new Location(), 10, 10);
            LoadedCommand = new Command(OnLoaded);
            TestCommand = new Command(OnTest);
        }

        private async void OnLoaded()
        {
            MapSpan lastRegion = _preferencesStore.Get(Constants.PreferencesStore.LastRegionKey, null);
            if (lastRegion != null)
            {
                MoveToRegion.Execute(lastRegion);
            }

            var locationPermission = await _permissionsService.CheckPermissionAsync<Permissions.LocationWhenInUse>();
            if (locationPermission.HasFlag(PermissionStatus.Granted))
            {
                var gpsInterval = _preferencesStore.Get(Constants.PreferencesStore.SettingsGpsInterval, Constants.DefaultSettings.GpsInterval);

                _gpsService.RaiseGpsLocationEvent += OnGpsNewLocation;
                if (!_gpsService.IsListening)
                {
                    await _gpsService.StartListeningAsync(gpsInterval, gpsInterval);
                }
            }
        }

        private void OnTest()
        {
            var nspan = CurrentRegion;
            var ncenter = new Location(nspan.Center.Latitude + 5, nspan.Center.Longitude);
            MoveToRegion.Execute(MapSpan.FromCenterAndRadius(ncenter, nspan.Radius));
        }

        private async void OnGpsNewLocation(object sender, GpsLocationEventArgs e)
        {
            var t = Toast.Make($"{e.Location}");
            await t.Show();
        }
    }
}
