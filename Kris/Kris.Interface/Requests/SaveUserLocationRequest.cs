namespace Kris.Interface
{
    public class SaveUserLocationRequest : RequestBase
    {
        public int UserId { get; set; }
        public UserLocation Location { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
