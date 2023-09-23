namespace Kris.Client.Common
{
    public class GpsLocationEventArgs : EventArgs
    {
        public Location Location { get; set; }
        public int RequestInterval { get; set; }

        public GpsLocationEventArgs(Location location, int requestInterval)
        {
            Location = location;
            RequestInterval = requestInterval;
        }
    }
}
