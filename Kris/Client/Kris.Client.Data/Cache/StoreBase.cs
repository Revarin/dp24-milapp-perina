using System.Text.Json;

namespace Kris.Client.Data.Cache;

public abstract class StoreBase : IPreferences
{
    private readonly JsonSerializerOptions _serializerOptions;

    protected StoreBase()
    {
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }

    public T Get<T>(string key, T defaultValue = default, string sharedName = null)
    {
        var json = Preferences.Get(key, "", sharedName);
        if (string.IsNullOrEmpty(json)) return defaultValue;
        return (T)JsonSerializer.Deserialize(json, typeof(T), _serializerOptions);
    }

    public void Set<T>(string key, T value, string sharedName = null)
    {
        var json = JsonSerializer.Serialize(value, _serializerOptions);
        Preferences.Set(key, json, sharedName);
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
