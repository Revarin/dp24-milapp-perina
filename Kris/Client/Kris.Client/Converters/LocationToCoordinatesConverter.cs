using CoordinateSharp;
using Kris.Client.Common.Enums;
using System.Globalization;

namespace Kris.Client.Converters;

public sealed class LocationToCoordinatesConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is LocationCoordinates lc)
        {
            var coordinate = new Coordinate(lc.Location.Latitude, lc.Location.Longitude);

            if (lc.CoordinateSystem == CoordinateSystem.LatLong) return coordinate.ToString();
            else if (lc.CoordinateSystem == CoordinateSystem.UTM) return coordinate.UTM.ToString();
            else if (lc.CoordinateSystem == CoordinateSystem.MGRS) return coordinate.MGRS.ToString();
            else return coordinate.MGRS.ToString();
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string text)
        {
            Coordinate coordinate = Coordinate.Parse(text);
            return new Location(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble());
        }

        return null;
    }
}
