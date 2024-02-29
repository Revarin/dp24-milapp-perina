namespace Kris.Interface.Models;

public sealed class ConnectionSettingsModel
{
    public int GpsRequestInterval { get; set; }
    public int PositionUploadFrequency { get; set; }
    public int PositionDownloadFrequency { get; set; }
    public int MapObjectDownloadFrequency { get; set; }
}
