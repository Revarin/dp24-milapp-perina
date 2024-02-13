namespace Kris.Common.Models;

public record GeoPosition
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }
}
