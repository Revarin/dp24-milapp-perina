using Kris.Client.Data.Models;
using Kris.Interface.Models;

namespace Kris.Client.Core.Mappers;

public interface ISettingsMapper
{
    ConnectionSettingsEntity Map(ConnectionSettingsModel model);
    ConnectionSettingsModel Map(ConnectionSettingsEntity entity);
    MapSettingsEntity Map(MapSettingsModel model);
    MapSettingsModel Map(MapSettingsEntity entity);
}
