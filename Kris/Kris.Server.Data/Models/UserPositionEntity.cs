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
    public required string PositionsData
    {
        get => string.Join('#', Positions ?? Array.Empty<GeoSpatialPosition>());
        set => Positions = value == null ?
            Array.Empty<GeoSpatialPosition>() :
            Array.ConvertAll(value.Split('#', StringSplitOptions.RemoveEmptyEntries), GeoSpatialPosition.Parse);
    }

    [NotMapped]
    public required GeoSpatialPosition[] Positions { get; set; } = new GeoSpatialPosition[3];
}
