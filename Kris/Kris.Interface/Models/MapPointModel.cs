using Kris.Common.Models;

namespace Kris.Interface.Models;

public class MapPointModel : MapObjectModel
{
    public required GeoPosition Position { get; set; }
    public required MapPointSymbol Symbol { get; set; }
}
