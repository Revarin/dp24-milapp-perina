using Kris.Client.Data.Models;
using Kris.Interface.Models;

namespace Kris.Client.Core.Mappers;

public sealed class SettingsMapper : ISettingsMapper
{
    public ConnectionSettingsEntity Map(ConnectionSettingsModel model)
    {
        return new ConnectionSettingsEntity
        {
            GpsInterval = TimeSpan.FromSeconds(model.GpsRequestInterval),
            PositionUploadMultiplier = model.PositionUploadFrequency,
            PositionDownloadInterval = TimeSpan.FromSeconds(model.PositionDownloadFrequency)
        };
    }

    public ConnectionSettingsModel Map(ConnectionSettingsEntity entity)
    {
        return new ConnectionSettingsModel
        {
            GpsRequestInterval = entity.GpsInterval.Seconds,
            PositionUploadFrequency = entity.PositionUploadMultiplier,
            PositionDownloadFrequency = entity.PositionDownloadInterval.Seconds
        };
    }
}
