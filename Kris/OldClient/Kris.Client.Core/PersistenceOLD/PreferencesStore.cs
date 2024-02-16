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

            Set<string>(Constants.PreferencesStore.LastRegionLocationKey, location);
            Set<string>(Constants.PreferencesStore.LastRegionDistanceKey, distance);
        }

        public MapSpan GetLastRegion()
        {
            var location = Get<string>(Constants.PreferencesStore.LastRegionLocationKey, null);
            var distance = Get<string>(Constants.PreferencesStore.LastRegionDistanceKey, null);

            if (string.IsNullOrEmpty(location) || string.IsNullOrEmpty(distance)) return null;

            return MapSpan.FromCenterAndRadius(
                JsonConvert.DeserializeObject<Location>(location),
                Distance.FromKilometers(JsonConvert.DeserializeObject<double>(distance)));
        }

        public ConnectionSettings GetConnectionSettings()
        {
            return new ConnectionSettings
            {
                UserId = Get<int>(Constants.ConnectionSettings.UserIdKey, -1),
                UserName = Get<string>(Constants.ConnectionSettings.UserNameKey),
                GpsInterval = Get<int>(Constants.ConnectionSettings.GpsIntervalKey, Constants.ConnectionSettings.DefaultGpsInterval),
                UsersLocationInterval = Get<int>(Constants.ConnectionSettings.UsersLocationIntervalKey, Constants.ConnectionSettings.DefaultUsersLocationInterval)
            };
        }

        public void SetConnectionSettings(ConnectionSettings settings)
        {
            Set<int>(Constants.ConnectionSettings.UserIdKey, settings.UserId);
            Set<string>(Constants.ConnectionSettings.UserNameKey, settings.UserName);
            Set<int>(Constants.ConnectionSettings.GpsIntervalKey, settings.GpsInterval);
            Set<int>(Constants.ConnectionSettings.UsersLocationIntervalKey, settings.UsersLocationInterval);
        }
    }
}
