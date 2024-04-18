using System.Globalization;

namespace Kris.Client.Converters;

public sealed class DeviceOrientationToStackOrientationConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DisplayOrientation display)
        {
            if (display == DisplayOrientation.Portrait) return StackOrientation.Vertical;
            else if (display == DisplayOrientation.Landscape) return StackOrientation.Horizontal;
        }
        return StackOrientation.Vertical;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
