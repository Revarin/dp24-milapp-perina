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
            PositionDownloadInterval = TimeSpan.FromSeconds(model.PositionDownloadFrequency),
            MapObjectDownloadInterval = TimeSpan.FromSeconds(model.MapObjectDownloadFrequency)
        };
    }

    public ConnectionSettingsModel Map(ConnectionSettingsEntity entity)
    {
        return new ConnectionSettingsModel
        {
            GpsRequestInterval = (int)entity.GpsInterval.TotalSeconds,
            PositionUploadFrequency = entity.PositionUploadMultiplier,
            PositionDownloadFrequency = (int)entity.PositionDownloadInterval.TotalSeconds,
            MapObjectDownloadFrequency = (int)entity.MapObjectDownloadInterval.TotalSeconds
        };
    }

    public MapSettingsEntity Map(MapSettingsModel model)
    {
        return new MapSettingsEntity
        {
            CoordinateSystem = model.CoordinateSystem,
            MapType = model.MapType
        };
    }

    public MapSettingsModel Map(MapSettingsEntity entity)
    {
        return new MapSettingsModel
        {
            CoordinateSystem = entity.CoordinateSystem.GetValueOrDefault(),
            MapType = entity.MapType.GetValueOrDefault()
        };
    }
}
