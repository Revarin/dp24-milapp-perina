using Kris.Client.Components.Map;
using Kris.Common.Enums;
using System.Reflection;

namespace Kris.Client.Utility;

public static class MapStyleLoader
{
    public static async Task<KrisMapStyle> LoadStyleAsync(KrisMapType mapStyle)
    {
        var assembly = Assembly.GetExecutingAssembly();
        Stream stream = null;

        try
        {
            switch (mapStyle)
            {
                case KrisMapType.StreetDark:
                default:
                    stream = assembly.GetManifestResourceStream("Kris.Client.Resources.Styles.MapMilitaryDark.json");
                    break;
            }

            using var streamReader = new StreamReader(stream);
            var jsonString = await streamReader.ReadToEndAsync();
            return new KrisMapStyle(jsonString);
        }
        finally
        {
            stream?.Dispose();
        }
    }
}
