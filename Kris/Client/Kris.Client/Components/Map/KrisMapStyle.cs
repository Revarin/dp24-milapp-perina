using Kris.Client.Data.Models.Database;
using Kris.Common.Enums;

namespace Kris.Client.Components.Map;

public sealed class KrisMapStyle
{
    public KrisMapType KrisMapType { get; init; }
    public string JsonStyle { get; init; }
    public Func<int, int, int, TileEntity> TileSource { get; set; }
}
