using Kris.Interface.Models;

namespace Kris.Interface.Responses;

public sealed class GetMapObjectsResponse : ResponseBase
{
    public required List<MapPointModel> MapPoints { get; set; }
    public required DateTime Resolved { get; set; }
}
