using Kris.Common.Enums;
using Kris.Common.Models;

namespace Kris.Interface.Requests;

public sealed class AddMapPointRequest : RequestBase
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public MapObjectType Type { get; set; }
    public required GeoPosition Position { get; set; }
    public required MapPointSymbol Symbol { get; set; }
}
