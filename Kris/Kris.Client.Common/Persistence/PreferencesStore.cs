using Microsoft.Maui.Maps;
using Newtonsoft.Json;

namespace Kris.Client.Common
{
    public class PreferencesStore
    {
        public void Set<T>(string key, T value)
        {
            var json = JsonConvert.SerializeObject(value);
            Preferences.Set(key, json);
        }

        public T Get<T>(string key)
        {
            var json = Preferences.Get(key, "");

            if (string.IsNullOrEmpty(json)) return default;

            return JsonConvert.DeserializeObject<T>(json);
        }

        public void Set(string key, MapSpan mapSpan)
        {
            var location = JsonConvert.SerializeObject(mapSpan.Center);
            var distance = JsonConvert.SerializeObject(mapSpan.Radius.Kilometers);

            Preferences.Set($"{key}-{Constants.PreferencesStore.MapSpanLocationKey}", location);
            Preferences.Set($"{key}-{Constants.PreferencesStore.MapSpanDistanceKey}", distance);
        }

        public MapSpan Get(string key)
        {
            string location = Preferences.Get($"{key}-{Constants.PreferencesStore.MapSpanLocationKey}", null);
            string distance = Preferences.Get($"{key}-{Constants.PreferencesStore.MapSpanDistanceKey}", null);

            if (string.IsNullOrEmpty(location) || string.IsNullOrEmpty(distance)) return default;

            return MapSpan.FromCenterAndRadius(
                JsonConvert.DeserializeObject<Location>(location),
                Distance.FromKilometers(JsonConvert.DeserializeObject<double>(distance)));
        }

        public void Clear(string key)
        {
            Preferences.Remove(key);
        }
    }
}
