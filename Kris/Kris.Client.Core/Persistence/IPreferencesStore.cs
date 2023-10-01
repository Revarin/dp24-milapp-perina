using Kris.Client.Common;
using Microsoft.Maui.Maps;

namespace Kris.Client.Core
{
    public interface IPreferencesStore : IPreferences
    {
        MapSpan GetLastRegion();
        void SetLastRegion(MapSpan value);

        UserSettings GetUserSettings();
        void SetUserSettings(UserSettings value);
        ConnectionSettings GetConnectionSettings();
        void SetConnectionSettings(ConnectionSettings value);
    }
}
