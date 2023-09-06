using System.Windows.Input;
using Microsoft.Maui.Maps;
using Kris.Client.Behaviors;

namespace Kris.Client.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private readonly MapSpan _currentRegion;
        public MapSpan CurrentRegion
        {
            get { return _currentRegion; }
            init { SetPropertyValue(ref _currentRegion, value); }
        }
        public MoveToRegionRequest MoveToRegion { get; init; } = new MoveToRegionRequest();

        public ICommand LoadedCommand { get; init; }
        public ICommand TestCommand { get; init; }

        public MapViewModel()
        {
            CurrentRegion = new MapSpan(new Location(), 10, 10);
            LoadedCommand = new Command(OnLoaded);
            TestCommand = new Command(OnTest);
        }

        private void OnLoaded()
        {
            MoveToRegion.Execute(MapSpan.FromCenterAndRadius(new Location(), Distance.FromKilometers(4000)));
        }

        private void OnTest()
        {
            var nspan = CurrentRegion;
            var ncenter = new Location(nspan.Center.Latitude + 5, nspan.Center.Longitude);
            MoveToRegion.Execute(MapSpan.FromCenterAndRadius(ncenter, nspan.Radius));
        }
    }
}
