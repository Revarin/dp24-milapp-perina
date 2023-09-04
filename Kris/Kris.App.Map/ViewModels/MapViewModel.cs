using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Kris.App.Common;
using Newtonsoft.Json;

namespace Kris.App.Map
{
    public class MapViewModel : ViewModelBase
    {
        private readonly PreferencesManager _preferencesManager;

        private MapSpan _currentRegion;
        public MapSpan CurrentRegion
        {
            get { return _currentRegion; }
            set { SetPropertyValue(ref _currentRegion, value); }
        }

        public MoveToRegionRequest MoveToRegion { get; } = new MoveToRegionRequest();


        public MapViewModel()
        {
            _preferencesManager = new PreferencesManager();

            // Cant do this, needs to be after initialization
            //var savedPosition = _preferencesManager.Get<MapSpan>("POSITION");
            //if (savedPosition != null)
            //{
            //    _position = savedPosition;
            //}
        }

        public void OnLoaded(object sender, System.EventArgs e)
        {
            var pos = new Position(0, 0);
            MoveToRegion.Execute(MapSpan.FromCenterAndRadius(pos, Distance.FromKilometers(2000)));
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetPropertyValue(ref _text, value); }
        }
        public ICommand DebugCommand => new Command(ShowDebugText);
        public void ShowDebugText()
        {
            var nspan = CurrentRegion;
            var x = new Position(nspan.Center.Latitude + 10, nspan.Center.Longitude);
            MoveToRegion.Execute(MapSpan.FromCenterAndRadius(x, nspan.Radius));

            Text = CurrentRegion?.LongitudeDegrees.ToString() ?? "ERROR";
        }
    }
}
