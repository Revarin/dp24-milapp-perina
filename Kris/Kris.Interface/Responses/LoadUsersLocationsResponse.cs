namespace Kris.Interface
{
    public class LoadUsersLocationsResponse : ResponseBase
    {
        public DateTime TimeStamp { get; set; }
        public IEnumerable<UserLocation> UserLocations { get; set; }
    }
}
