using Kris.Client.Data;

namespace Kris.Client.Core
{
    public class UsersLocationsEventArgs : EventArgs
    {
        public DateTime TimeStamp { get; set; }
        public IEnumerable<UserLocation> UserLocations { get; set; }

        public UsersLocationsEventArgs(DateTime timeStamp, IEnumerable<UserLocation> userLocations)
        {
            TimeStamp = timeStamp;
            UserLocations = userLocations;
        }
    }
}
