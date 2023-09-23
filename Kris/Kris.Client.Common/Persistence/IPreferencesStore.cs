﻿using Microsoft.Maui.Maps;

namespace Kris.Client.Common
{
    public interface IPreferencesStore : IPreferences
    {
        void Set(string key, MapSpan value);
        MapSpan Get(string key, MapSpan defaultValue = null);
    }
}