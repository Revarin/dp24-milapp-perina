using System.Globalization;

namespace Kris.Client.Converters;

public sealed class DeviceOrientationToStackOrientationInverseConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DisplayOrientation display)
        {
            if (display == DisplayOrientation.Portrait) return StackOrientation.Horizontal;
            else if (display == DisplayOrientation.Landscape) return StackOrientation.Vertical;
        }
        return StackOrientation.Horizontal;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
