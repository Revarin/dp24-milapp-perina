using System.Globalization;

namespace Kris.Common.Extensions;

public static class DateTimeExtensions
{
    public static string ToISOString(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
    }
}
