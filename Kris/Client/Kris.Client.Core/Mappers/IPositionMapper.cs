using Kris.Common.Models;

namespace Kris.Client.Core.Mappers;

public interface IPositionMapper
{
    GeoSpatialPosition Map(Location location);
    Location Map(GeoSpatialPosition position);
}
