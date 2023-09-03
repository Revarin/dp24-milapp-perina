using Xamarin.Essentials;
using Newtonsoft.Json;
using System;

namespace Kris.App.Common
{
    public class PreferencesManager
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

        public void Clear(string key)
        {
            Preferences.Remove(key);
        }
    }
}
