using System.ComponentModel.DataAnnotations.Schema;

namespace Kris.Server.Data
{
    [Table("LOCATION_RECORDS")]
    public class LocationRecordEntity : EntityBase<int>
    {
        [Column("USER_ID")]
        public int UserId { get; set; }
        [Column("UPDATED_DATE", TypeName = "datetime")]
        public DateTime UpdatedDate { get; set; }
        [Column("LATITUDE")]
        public double Latitude { get; set; }
        [Column("LONGTITUDE")]
        public double Longitude { get; set; }
    }
}
