using Kris.Client.Data.Models.Database;
using Kris.Common.Enums;

namespace Kris.Client.Components.Map;

public sealed class KrisMapStyle
{
    public KrisMapType KrisMapType { get; init; }
    public string JsonStyle { get; init; }

    public Func<int, int, int, TileEntity> TileSource { get; set; }

    public KrisMapStyle(KrisMapType krisMapType)
    {
        KrisMapType = krisMapType;
    }

    public KrisMapStyle(KrisMapType krisMapType, string jsonStyle)
    {
        KrisMapType = krisMapType;
        JsonStyle = jsonStyle;
    }

    public KrisMapStyle(KrisMapType krisMapType, Func<int, int, int, TileEntity> tileSource)
    {
        KrisMapType = krisMapType;
        TileSource = tileSource;
    }
}
