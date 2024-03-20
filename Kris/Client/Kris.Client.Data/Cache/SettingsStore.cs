using Kris.Client.Data.Models;

namespace Kris.Client.Data.Cache;

public sealed class SettingsStore : StoreBase, ISettingsStore
{
    private const string ConnectionSettingsKey = "kris-settings-connection";
    private const string MapSettingsKey = "kris-settings-map";
    private const string MapTilesSourcePathKey = "kris-settings-map-tilesource";

    public ConnectionSettingsEntity GetConnectionSettings()
    {
        return Get<ConnectionSettingsEntity>(ConnectionSettingsKey);
    }

    public MapSettingsEntity GetMapSettings()
    {
        return Get<MapSettingsEntity>(MapSettingsKey);
    }

    public string GetMapTilesSourcePath()
    {
        return Get<string>(MapTilesSourcePathKey);
    }

    public void StoreConnectionSettings(ConnectionSettingsEntity settings)
    {
        Set(ConnectionSettingsKey, settings);
    }

    public void StoreMapSettings(MapSettingsEntity settings)
    {
        Set(MapSettingsKey, settings);
    }

    public void StoreMapTilesSourcePath(string path)
    {
        Set(MapTilesSourcePathKey, path);
    }
}
