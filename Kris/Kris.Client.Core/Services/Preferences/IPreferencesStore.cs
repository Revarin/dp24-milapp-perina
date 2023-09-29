using Kris.Client.Common;
using Microsoft.Maui.Maps;

namespace Kris.Client.Core
{
    public interface IPreferencesStore : IPreferences
    {
        void Set(string key, MapSpan value);
        MapSpan Get(string key, MapSpan defaultValue = null);

        UserSettings GetUserSettings();
    }
}
