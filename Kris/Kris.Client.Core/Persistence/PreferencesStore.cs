using Microsoft.Maui.Maps;
using Newtonsoft.Json;
using Kris.Client.Common;

namespace Kris.Client.Core
{
    public class PreferencesStore : PreferencesBase, IPreferencesStore
    {
        public void Set(string key, MapSpan value)
        {
            var location = JsonConvert.SerializeObject(value.Center);
            var distance = JsonConvert.SerializeObject(value.Radius.Kilometers);

            Preferences.Set($"{key}-{Constants.PreferencesStore.MapSpanLocationKey}", location);
            Preferences.Set($"{key}-{Constants.PreferencesStore.MapSpanDistanceKey}", distance);
        }

        public MapSpan Get(string key, MapSpan defaultValue = default)
        {
            var location = Get<string>($"{key}-{Constants.PreferencesStore.MapSpanLocationKey}", null);
            var distance = Get<string>($"{key}-{Constants.PreferencesStore.MapSpanDistanceKey}", null);

            if (string.IsNullOrEmpty(location) || string.IsNullOrEmpty(distance)) return defaultValue;

            return MapSpan.FromCenterAndRadius(
                JsonConvert.DeserializeObject<Location>(location),
                Distance.FromKilometers(JsonConvert.DeserializeObject<double>(distance)));
        }

        public UserSettings GetUserSettings()
        {
            return new UserSettings
            {
                UserId = Get<int>(Constants.UserSettings.UserId, -1),
                UserName = Get<string>(Constants.UserSettings.UserName)
            };
        }

        public void SetUserSettings(UserSettings settings)
        {
            Set<int>(Constants.UserSettings.UserId, settings.UserId);
            Set<string>(Constants.UserSettings.UserName, settings.UserName);
        }

        public ConnectionSettings GetConnectionSettings()
        {
            return new ConnectionSettings
            {
                GpsRequestInterval = Get<int>(Constants.ConnectionSettings.GpsInterval, -1)
            };
        }

        public void SetConnectionSettings(ConnectionSettings settings)
        {
            Set<int>(Constants.ConnectionSettings.GpsInterval, settings.GpsRequestInterval);
        }
    }
}
