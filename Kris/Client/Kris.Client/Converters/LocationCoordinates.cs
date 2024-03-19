using Kris.Common.Enums;

namespace Kris.Client.Converters;

public sealed class LocationCoordinates
{
    public Location Location { get; set; }
    public CoordinateSystem CoordinateSystem { get; set; }

    public LocationCoordinates()
    {
        CoordinateSystem = CoordinateSystem.LatLong;
        Location = new Location();
    }
}
