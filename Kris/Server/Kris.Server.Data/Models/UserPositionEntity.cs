using Kris.Common.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kris.Server.Data.Models;

public class UserPositionEntity : EntityBase
{
    public required Guid SessionUserId { get; set; }
    public SessionUserEntity? SessionUser { get; set; }
    public required DateTime Updated { get; set; }
    public string? Position0Data
    {
        get => Positions[0]?.ToString() ?? null;
        set => Positions[0] = value == null ? null : GeoSpatialPosition.Parse(value);
    }
    public string? Position1Data
    {
        get => Positions[1]?.ToString() ?? null;
        set => Positions[1] = value == null ? null : GeoSpatialPosition.Parse(value);
    }
    public string? Position2Data
    {
        get => Positions[2]?.ToString() ?? null;
        set => Positions[2] = value == null ? null : GeoSpatialPosition.Parse(value);
    }

    [NotMapped]
    public required GeoSpatialPosition?[] Positions { get; set; } = new GeoSpatialPosition?[3];
}
