using Newtonsoft.Json;

namespace Kris.Client.Common
{
    public abstract class PreferencesBase : IPreferences
    {
        public void Set<T>(string key, T value, string sharedName = null)
        {
            var json = JsonConvert.SerializeObject(value);
            Preferences.Set(key, json, sharedName);
        }

        public T Get<T>(string key, T defaultValue = default, string sharedName = null)
        {
            var json = Preferences.Get(key, "", sharedName);

            if (string.IsNullOrEmpty(json)) return defaultValue;

            return JsonConvert.DeserializeObject<T>(json);
        }

        public bool ContainsKey(string key, string sharedName = null)
        {
            return Preferences.ContainsKey(key, sharedName);
        }

        public void Remove(string key, string sharedName = null)
        {
            Preferences.Remove(key, sharedName);
        }

        public void Clear(string sharedName = null)
        {
            Preferences.Clear(sharedName);
        }
    }
}
