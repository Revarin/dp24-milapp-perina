using Kris.Client.Components.Map;
using System.Globalization;

namespace Kris.Client.Converters;

public sealed class KrisMapStyleToTextColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is KrisMapStyle kms)
        {
            if (kms.KrisMapType == Kris.Common.Enums.KrisMapType.StreetLight) return Colors.Black;
            else return Colors.White;
        }

        return Colors.White;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
