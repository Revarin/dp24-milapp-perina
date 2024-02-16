using System.ComponentModel;

namespace Kris.Client.Data
{
    public class MapPin : MapObject
    {
        public string Address { get; set; }
        public Location Location { get; set; }
        public ImageSource ImageSource { get; set; }
        public CustomPinType PinType { get; set; }
    }
}
