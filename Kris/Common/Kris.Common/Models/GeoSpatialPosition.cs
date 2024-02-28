namespace Kris.Common.Models;

public record GeoSpatialPosition : GeoPosition
{
    public DateTime Timestamp { get; set; }

    public override string ToString()
    {
        return string.Join(';', Timestamp, Latitude, Longitude, Altitude);
    }

    public static GeoSpatialPosition Parse(string data)
    {
        var split = data.Split(';');

        return new GeoSpatialPosition
        {
            Timestamp = DateTime.Parse(split[0]),
            Latitude = double.Parse(split[1]),
            Longitude = double.Parse(split[2]),
            Altitude = double.Parse(split[3])
        };
    }
}
