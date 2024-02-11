using Kris.Common.Models;

namespace Kris.Interface.Models;

public sealed class UserPositionModel
{
    public required Guid userId { get; set; }
    public required string userName { get; set; }
    public required List<GeoSpatialPosition> Positions { get; set; }
}
