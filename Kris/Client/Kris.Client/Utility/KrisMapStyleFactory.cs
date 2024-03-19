using Kris.Client.Components.Map;
using Kris.Client.Data.Models.Database;
using Kris.Common.Enums;
using System.Reflection;

namespace Kris.Client.Utility;

public static class KrisMapStyleFactory
{
    public static async Task<KrisMapStyle> CreateStyleAsync(KrisMapType mapStyle, Func<int, int, int, TileEntity> tileSource = null)
    {
        var assembly = Assembly.GetExecutingAssembly();
        Stream stream = null;

        try
        {
            switch (mapStyle)
            {
                case KrisMapType.Satelite:
                    return new KrisMapStyle(mapStyle);
                case KrisMapType.Custom:
                    return new KrisMapStyle(mapStyle, tileSource);
                case KrisMapType.StreetLight:
                    stream = assembly.GetManifestResourceStream("Kris.Client.Resources.Styles.MapMilitaryLight.json");
                    break;
                case KrisMapType.StreetDark:
                default:
                    stream = assembly.GetManifestResourceStream("Kris.Client.Resources.Styles.MapMilitaryDark.json");
                    break;
            }

            using var streamReader = new StreamReader(stream);
            var jsonString = await streamReader.ReadToEndAsync();
            return new KrisMapStyle(mapStyle, jsonString);
        }
        finally
        {
            stream?.Dispose();
        }
    }
}
