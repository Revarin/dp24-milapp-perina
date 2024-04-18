using CoordinateSharp;
using Kris.Common.Enums;
using System.Globalization;

namespace Kris.Client.Converters;

public sealed class LocationToFormatedCoordinatesConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is LocationCoordinates lc)
        {
            var coordinate = new Coordinate(lc.Location.Latitude, lc.Location.Longitude);

            if (lc.CoordinateSystem == CoordinateSystem.LatLong)
            {
                var latLongString = coordinate.ToString();
                var chars = latLongString.ToCharArray();
                chars[latLongString.IndexOf('"') + 1] = '\n';
                return new string(chars);
            }
            else if (lc.CoordinateSystem == CoordinateSystem.UTM)
            {
                var utmString = coordinate.UTM.ToString();
                utmString = utmString.Replace("m", string.Empty);
                return utmString;
            }
            else if (lc.CoordinateSystem == CoordinateSystem.MGRS)
            {
                var mgrsString = coordinate.MGRS.ToString();
                return mgrsString;
            }
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
