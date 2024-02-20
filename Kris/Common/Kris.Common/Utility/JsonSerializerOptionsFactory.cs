using System.Text.Json;

namespace Kris.Common.Utility;

public static class JsonSerializerOptionsFactory
{
    public static JsonSerializerOptions CreateHttpJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,        
        };
    }

    public static JsonSerializerOptions CreateStorageJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
        };
    }
}
