using Kris.Client.Data.Models;
using Kris.Interface.Models;

namespace Kris.Client.Core.Mappers;

public interface ISettingsMapper
{
    ConnectionSettingsEntity Map(ConnectionSettingsModel model);
}
