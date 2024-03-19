using Kris.Common.Enums;

namespace Kris.Client.Data.Models;

public sealed class MapSettingsEntity
{
    public CoordinateSystem? CoordinateSystem { get; set; }
    public KrisMapType? MapType { get; set; }
    public string? CustomMapTilesDatabasePath { get; set; }
}
