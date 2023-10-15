namespace Kris.Interface
{
    public class LoadUsersLocationsRequest : RequestBase
    {
        public int UserId { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
