using Kris.Common.Enums;
using Kris.Common.Models;

namespace Kris.Interface.Models;

public sealed class MapPointDetailModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required MapObjectType Type { get; set; }
    public required DateTime Created { get; set; }
    public required UserModel User { get; set; }
    public required GeoPosition Position { get; set; }
    public required MapPointSymbol Symbol { get; set; }
    public string? Description { get; set; }
}
