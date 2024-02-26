using Kris.Client.Data.Models;

namespace Kris.Client.Data.Cache;

public interface ISettingsStore
{
    void StoreConnectionSettings(ConnectionSettingsEntity settings);
    ConnectionSettingsEntity GetConnectionSettings();
}
