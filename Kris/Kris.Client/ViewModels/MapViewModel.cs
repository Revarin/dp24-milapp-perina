using System.Windows.Input;
using Microsoft.Maui.Maps;
using Kris.Client.Behaviors;
using Kris.Client.Common;
using CommunityToolkit.Maui.Alerts;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls.Maps;

namespace Kris.Client.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IPreferencesStore _preferencesStore;
        private readonly IGpsService _gpsService;

        private MapSpan _currentRegion;
        public MapSpan CurrentRegion
        {
            get { return _currentRegion; }
            set { SetPropertyValue(ref _currentRegion, value); }
        }
        private ObservableCollection<Pin> _pinsSource;
        public ObservableCollection<Pin> PinsSource
        {
            get { return _pinsSource; }
            set { SetPropertyValue(ref _pinsSource, value); }
        }
        public MoveToRegionRequest MoveToRegion { get; init; } = new MoveToRegionRequest();

        public ICommand LoadedCommand { get; init; }

        public MapViewModel(IPermissionsService permissionsService, IPreferencesStore preferencesStore, IGpsService gpsService)
        {
            _permissionsService = permissionsService;
            _preferencesStore = preferencesStore;
            _gpsService = gpsService;

            CurrentRegion = new MapSpan(new Location(), 10, 10);
            LoadedCommand = new Command(OnLoaded);
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
                    _gpsService.SetupListener(gpsInterval, gpsInterval);
                    await _gpsService.StartListeningAsync();
                }
            }
        }

        private async void OnGpsNewLocation(object sender, GpsLocationEventArgs e)
        {
            // DEBUG
            var t = Toast.Make($"[{e.RequestInterval}]{e.Location}");
            await t.Show();

            PinsSource.Clear();
            PinsSource.Add(new Pin
            {
                Location = e.Location,
                Label = "User",
            });
        }
    }
}
