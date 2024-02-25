using Kris.Client.Data.Models;

namespace Kris.Client.Data.Cache;

public sealed class SettingsStore : StoreBase, ISettingsStore
{
    private const string ConnectionSettingsKey = "kris-settings-connection";

    public UserConnectionSettingsEntity GetConnectionSettings()
    {
        return Get<UserConnectionSettingsEntity>(ConnectionSettingsKey);
    }

    public void StoreConnectionSettings(UserConnectionSettingsEntity settings)
    {
        Set(ConnectionSettingsKey, settings);
    }
}
