using Kris.Client.Common.Enums;

namespace Kris.Client.Converters;

public sealed class LocationCoordinates
{
    public Location Location { get; set; }
    public CoordinateSystem CoordinateSystem { get; set; }
}
