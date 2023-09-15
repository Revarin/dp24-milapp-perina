using System.Windows.Input;
using Microsoft.Maui.Maps;
using Kris.Client.Behaviors;
using Kris.Client.Common;

namespace Kris.Client.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IPreferencesStore _preferencesStore;

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

        public MapViewModel(IPermissionsService permissionsService, IPreferencesStore preferencesStore)
        {
            _permissionsService = permissionsService;
            _preferencesStore = preferencesStore;

            ShowingUserLocation = false;
            CurrentRegion = new MapSpan(new Location(), 10, 10);
            LoadedCommand = new Command(OnLoaded);
            TestCommand = new Command(OnTest);
        }

        private async void OnLoaded()
        {
            var locationPermission = await _permissionsService.CheckPermissionAsync<Permissions.LocationWhenInUse>();
            ShowingUserLocation = locationPermission.HasFlag(PermissionStatus.Granted);

            MapSpan lastRegion = _preferencesStore.Get(Constants.PreferencesStore.LastRegionKey, null);
            if (lastRegion != null)
            {
                MoveToRegion.Execute(lastRegion);
            }
        }

        private void OnTest()
        {
            var nspan = CurrentRegion;
            var ncenter = new Location(nspan.Center.Latitude + 5, nspan.Center.Longitude);
            MoveToRegion.Execute(MapSpan.FromCenterAndRadius(ncenter, nspan.Radius));
        }
    }
}
