using Kris.Common.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kris.Server.Data.Models;

public class UserPositionEntity : EntityBase
{
    [NotMapped]
    private new Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public required Guid SessionId { get; set; }
    public SessionUserEntity? SessionUser { get; set; }
    public required DateTime Updated { get; set; }
    public GeoSpatialPosition? Position_0 { get; set; }
    public GeoSpatialPosition? Position_1 { get; set; }
    public GeoSpatialPosition? Position_2 { get; set; }
}
