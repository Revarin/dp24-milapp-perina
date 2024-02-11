namespace Kris.Common.Models;

public struct GeoSpatialPosition
{
    public DateTime Timestamp { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }

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
