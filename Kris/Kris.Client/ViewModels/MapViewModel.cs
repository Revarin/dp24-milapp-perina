using System.Windows.Input;
using Microsoft.Maui.Maps;
using Kris.Client.Behaviors;
using Kris.Client.Common;

namespace Kris.Client.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IAlertService _alertService;
        private readonly IPreferencesStore _preferencesStore;

        private MapSpan _currentRegion;
        public MapSpan CurrentRegion
        {
            get { return _currentRegion; }
            set { SetPropertyValue(ref _currentRegion, value); }
        }
        public MoveToRegionRequest MoveToRegion { get; init; } = new MoveToRegionRequest();

        public ICommand AppearingCommand { get; init; }
        public ICommand LoadedCommand { get; init; }
        public ICommand TestCommand { get; init; }

        public MapViewModel(IPermissionsService permissionsService, IAlertService alertService, IPreferencesStore preferencesStore)
        {
            _permissionsService = permissionsService;
            _alertService = alertService;
            _preferencesStore = preferencesStore;

            CurrentRegion = new MapSpan(new Location(), 10, 10);
            AppearingCommand = new Command(OnAppearing);
            LoadedCommand = new Command(OnLoaded);
            TestCommand = new Command(OnTest);
        }

        private async void OnAppearing()
        {
            var status = await _permissionsService.CheckAndRequestPermissionAsync<Permissions.LocationWhenInUse>();
            if (!status.HasFlag(PermissionStatus.Granted))
            {
                await _alertService.ShowAlertAsync(I18n.Keys.MapLocationPermissionDeniedTitle, I18n.Keys.MapLocationPermissionDeniedMessage);
            }
        }

        private void OnLoaded()
        {
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
