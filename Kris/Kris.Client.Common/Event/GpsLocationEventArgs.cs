namespace Kris.Client.Common
{
    public class GpsLocationEventArgs : EventArgs
    {
        public Location Location { get; set; }

        public GpsLocationEventArgs(Location location)
        {
            Location = location;
        }
    }
}
