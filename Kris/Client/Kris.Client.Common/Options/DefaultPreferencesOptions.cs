using Kris.Common;
using Kris.Common.Enums;

namespace Kris.Client.Common.Options;

public sealed class DefaultPreferencesOptions : IOptions
{
    public static string Section => "DefaultPreferences";

    public int GpsInterval { get; init; }
    public int PositionUploadFrequency { get; init; }
    public int PositionDownloadFrequency { get; init; }
    public int MapObjectDownloadFrequency { get; init; }
    public CoordinateSystem CoordinateSystem { get; init; }
}
