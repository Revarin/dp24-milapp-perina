using Microsoft.Maui.Maps;
using Newtonsoft.Json;
using Kris.Client.Common;

namespace Kris.Client.Core
{
    public class PreferencesStore : PreferencesBase, IPreferencesStore
    {
        public void SetLastRegion(MapSpan value)
        {
            var location = JsonConvert.SerializeObject(value.Center);
            var distance = JsonConvert.SerializeObject(value.Radius.Kilometers);

            Set<string>(Constants.PreferencesStore.LastRegionLocation, location);
            Set<string>(Constants.PreferencesStore.LastRegionDistance, distance);
        }

        public MapSpan GetLastRegion()
        {
            var location = Get<string>(Constants.PreferencesStore.LastRegionLocation, null);
            var distance = Get<string>(Constants.PreferencesStore.LastRegionDistance, null);

            if (string.IsNullOrEmpty(location) || string.IsNullOrEmpty(distance)) return null;

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
                GpsInterval = Get<int>(Constants.ConnectionSettings.GpsInterval, -1)
            };
        }

        public void SetConnectionSettings(ConnectionSettings settings)
        {
            Set<int>(Constants.ConnectionSettings.GpsInterval, settings.GpsInterval);
        }
    }
}
