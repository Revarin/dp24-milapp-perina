using System.Windows.Input;
using Microsoft.Maui.Maps;
using Kris.Client.Behaviors;
using Kris.Client.Common;

namespace Kris.Client.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private readonly IPreferencesStore _preferencesStore;

        private MapSpan _currentRegion;
        public MapSpan CurrentRegion
        {
            get { return _currentRegion; }
            set { SetPropertyValue(ref _currentRegion, value); }
        }
        public MoveToRegionRequest MoveToRegion { get; init; } = new MoveToRegionRequest();

        public ICommand LoadedCommand { get; init; }
        public ICommand UnloadedCommand { get; init; }
        public ICommand TestCommand { get; init; }

        public MapViewModel(IPreferencesStore preferencesStore)
        {
            _preferencesStore = preferencesStore;

            CurrentRegion = new MapSpan(new Location(), 10, 10);
            LoadedCommand = new Command(OnLoaded);
            UnloadedCommand = new Command(OnUnloaded);
            TestCommand = new Command(OnTest);
        }

        private void OnLoaded()
        {
            MapSpan lastRegion = _preferencesStore.Get(Constants.PreferencesStore.LastRegionKey, null);
            if (lastRegion != null)
            {
                MoveToRegion.Execute(lastRegion);
            }
        }

        private void OnUnloaded()
        {
            _preferencesStore.Set(Constants.PreferencesStore.LastRegionKey, CurrentRegion);
        }

        private void OnTest()
        {
            var nspan = CurrentRegion;
            var ncenter = new Location(nspan.Center.Latitude + 5, nspan.Center.Longitude);
            MoveToRegion.Execute(MapSpan.FromCenterAndRadius(ncenter, nspan.Radius));
        }
    }
}
