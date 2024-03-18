using Kris.Common.Enums;

namespace Kris.Interface.Models;

public sealed class MapSettingsModel
{
    public CoordinateSystem CoordinateSystem { get; set; } 
    public KrisMapType MapType { get; set; }
}
