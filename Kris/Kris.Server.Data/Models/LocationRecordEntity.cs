namespace Kris.Server.Data
{
    public class LocationRecordEntity : EntityBase<int>
    {
        public int UserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
