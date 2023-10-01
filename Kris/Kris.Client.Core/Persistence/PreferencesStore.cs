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

        public ConnectionSettings GetConnectionSettings()
        {
            return new ConnectionSettings
            {
                UserId = Get<int>(Constants.ConnectionSettings.UserId, -1),
                UserName = Get<string>(Constants.ConnectionSettings.UserName),
                GpsInterval = Get<int>(Constants.ConnectionSettings.GpsInterval, Constants.DefaultSettings.GpsInterval)
            };
        }

        public void SetConnectionSettings(ConnectionSettings settings)
        {
            Set<int>(Constants.ConnectionSettings.UserId, settings.UserId);
            Set<string>(Constants.ConnectionSettings.UserName, settings.UserName);
            Set<int>(Constants.ConnectionSettings.GpsInterval, settings.GpsInterval);
        }
    }
}
