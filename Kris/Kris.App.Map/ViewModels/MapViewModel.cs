using Kris.App.Common;
using Xamarin.Forms.Maps;

namespace Kris.App.Map
{
    public class MapViewModel : ViewModelBase
    {
        public Xamarin.Forms.Maps.Map Map { get; set; }

        //private Position _position;
        //public Position Position
        //{
        //    get { return _position; }
        //    set { SetPropertyValue(ref _position, value); }
        //}

        public MapViewModel()
        {
            Map = new Xamarin.Forms.Maps.Map();
        }
    }
}
