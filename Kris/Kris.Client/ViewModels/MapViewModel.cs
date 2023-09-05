using Microsoft.Maui.Maps;
using System.Windows.Input;

namespace Kris.Client.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private MapSpan _currentRegion;
        public MapSpan CurrentRegion
        {
            get { return _currentRegion; }
            set { SetPropertyValue(ref _currentRegion, value); }
        }

        public ICommand TestCommand { get; private set; }

        public MapViewModel()
        {
            CurrentRegion = MapSpan.FromCenterAndRadius(new Location(), Distance.FromKilometers(4000));
            TestCommand = new Command(Test);
        }

        private void Test()
        {
            var nspan = CurrentRegion;
            var ncenter = new Location(nspan.Center.Latitude + 5, nspan.Center.Longitude);
            CurrentRegion = MapSpan.FromCenterAndRadius(ncenter, nspan.Radius);
        }
    }
}
