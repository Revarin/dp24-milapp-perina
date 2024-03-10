using Kris.Interface.Models;

namespace Kris.Interface.Responses;

public sealed class GetMapObjectsResponse : Response
{
    public List<MapPointListModel> MapPoints { get; set; }
    public DateTime Resolved { get; set; }
}
