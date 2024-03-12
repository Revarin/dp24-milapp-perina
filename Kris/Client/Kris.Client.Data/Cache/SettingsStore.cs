using Kris.Client.Data.Models;

namespace Kris.Client.Data.Cache;

public sealed class SettingsStore : StoreBase, ISettingsStore
{
    private const string ConnectionSettingsKey = "kris-settings-connection";
    private const string MapSettingsKey = "kris-settings-map";

    public ConnectionSettingsEntity GetConnectionSettings()
    {
        return Get<ConnectionSettingsEntity>(ConnectionSettingsKey);
    }

    public MapSettingsEntity GetMapSettings()
    {
        return Get<MapSettingsEntity>(MapSettingsKey);
    }

    public void StoreConnectionSettings(ConnectionSettingsEntity settings)
    {
        Set(ConnectionSettingsKey, settings);
    }

    public void StoreMapSettings(MapSettingsEntity settings)
    {
        Set(MapSettingsKey, settings);
    }
}
