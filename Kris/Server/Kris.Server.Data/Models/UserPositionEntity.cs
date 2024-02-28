using Kris.Common.Models;

namespace Kris.Server.Data.Models;

public class UserPositionEntity : EntityBase
{
    public SessionUserEntity? SessionUser { get; set; }
    public required DateTime Updated { get; set; }
    public GeoSpatialPosition? Position_0 { get; set; }
    public GeoSpatialPosition? Position_1 { get; set; }
    public GeoSpatialPosition? Position_2 { get; set; }
}
