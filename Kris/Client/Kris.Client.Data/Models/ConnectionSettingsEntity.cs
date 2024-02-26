namespace Kris.Client.Data.Models;

public sealed class ConnectionSettingsEntity
{
    public TimeSpan GpsInterval { get; set; }
    public int PositionUploadMultiplier { get; set; }
    public TimeSpan PositionDownloadInterval { get; set; }
}
