using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Kris.App.Common;

namespace Kris.App.Map
{
    public class MapViewModel : ViewModelBase
    {
        private readonly PreferencesManager _preferencesManager;

        private MapSpan _position;
        public MapSpan Position
        {
            get { return _position; }
            set
            { 
                SetPropertyValue(ref _position, value);
                _preferencesManager.Set("POSITION", _position);
            }
        }

        public MapViewModel()
        {
            _preferencesManager = new PreferencesManager();

            // Cant do this, needs to be after initialization
            var savedPosition = _preferencesManager.Get<MapSpan>("POSITION");
            if (savedPosition != null)
            {
                _position = savedPosition;
            }

            Position = new MapSpan(Position.Center, 10, 10);
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
            Text = Position?.ToString() ?? "ERROR";
        }
    }
}
