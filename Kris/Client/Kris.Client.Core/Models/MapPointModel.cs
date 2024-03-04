using Kris.Common.Models;

namespace Kris.Client.Core.Models;

public sealed class MapPointModel : MapObjectModel
{
    public Location Location { get; set; }
    public MapPointSymbol Symbol { get; set; }
}
