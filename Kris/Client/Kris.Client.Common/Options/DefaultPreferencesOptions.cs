using Kris.Common;

namespace Kris.Client.Common.Options;

public sealed class DefaultPreferencesOptions : IOptions
{
    public static string Section => "DefaultPreferences";

    public int GpsInterval { get; set; }
    public int PositionUploadFrequency { get; set; }
    public int PositionDownloadFrequency { get; set; }
}
