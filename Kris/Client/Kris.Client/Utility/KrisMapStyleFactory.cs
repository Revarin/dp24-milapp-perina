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
                    break;
                case KrisMapType.Military:
                    stream = assembly.GetManifestResourceStream("Kris.Client.Resources.Styles.MapMilitaryLight.json");
                    break;
                case KrisMapType.StreetLight:
                    stream = assembly.GetManifestResourceStream("Kris.Client.Resources.Styles.MapMilitaryLight.json");
                    break;
                case KrisMapType.StreetDark:
                default:
                    stream = assembly.GetManifestResourceStream("Kris.Client.Resources.Styles.MapMilitaryDark.json");
                    break;
            }

            if (stream == null) return new KrisMapStyle { KrisMapType = mapStyle };

            using var streamReader = new StreamReader(stream);
            var jsonString = await streamReader.ReadToEndAsync();
            return new KrisMapStyle
            {
                KrisMapType = mapStyle,
                JsonStyle = jsonString,
                TileSource = tileSource
            };
        }
        finally
        {
            stream?.Dispose();
        }
    }
}
