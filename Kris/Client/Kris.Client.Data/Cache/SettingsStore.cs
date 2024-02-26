using Kris.Client.Data.Models;

namespace Kris.Client.Data.Cache;

public sealed class SettingsStore : StoreBase, ISettingsStore
{
    private const string ConnectionSettingsKey = "kris-settings-connection";

    public ConnectionSettingsEntity GetConnectionSettings()
    {
        return Get<ConnectionSettingsEntity>(ConnectionSettingsKey);
    }

    public void StoreConnectionSettings(ConnectionSettingsEntity settings)
    {
        Set(ConnectionSettingsKey, settings);
    }
}
