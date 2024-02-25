using Kris.Common.Models;

namespace Kris.Client.Core.Mappers;

public sealed class PositionMapper : IPositionMapper
{
    public GeoSpatialPosition Map(Location location)
    {
        return new GeoSpatialPosition
        {
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            Altitude = location.Altitude ?? 0,
            Timestamp = DateTime.UtcNow
        };
    }

    public Location Map(GeoSpatialPosition position)
    {
        return new Location(position.Latitude, position.Longitude, position.Altitude);
    }

    public Models.UserPositionModel Map(Interface.Models.UserPositionModel position)
    {
        return new Models.UserPositionModel
        {
            UserId = position.UserId,
            UserName = position.UserName,
            Positions = position.Positions,
            Updated = position.Updated
        };
    }
}
