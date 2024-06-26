﻿using Kris.Client.Data.Models;

namespace Kris.Client.Data.Cache;

public interface ISettingsStore
{
    void StoreConnectionSettings(ConnectionSettingsEntity settings);
    ConnectionSettingsEntity GetConnectionSettings();
    void StoreMapSettings(MapSettingsEntity settings);
    MapSettingsEntity GetMapSettings();
    void StoreMapTilesSourcePath(string path);
    string GetMapTilesSourcePath();
}
