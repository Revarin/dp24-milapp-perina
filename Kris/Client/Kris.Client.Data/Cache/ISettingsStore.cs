using Kris.Client.Data.Models;

namespace Kris.Client.Data.Cache;

public interface ISettingsStore
{
    void StoreConnectionSettings(UserConnectionSettingsEntity settings);
    UserConnectionSettingsEntity GetConnectionSettings();
}
