using Kris.Common.Models;

namespace Kris.Client.Core.Mappers;

public sealed class PositionMapper : IPositionMapper
{
    public GeoSpatialPosition Map(Location location)
    {
        if (location == null) throw new ArgumentNullException("location");
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
        if (position == null) return null;
        return new Location(position.Latitude, position.Longitude, position.Altitude);
    }

    public Models.UserPositionModel Map(Interface.Models.UserPositionModel position)
    {
        if (position == null) throw new ArgumentNullException("position");
        return new Models.UserPositionModel
        {
            UserId = position.UserId,
            UserName = position.UserName,
            Positions = position.Positions.Select(Map).ToList(),
            Updated = position.Updated
        };
    }
}
