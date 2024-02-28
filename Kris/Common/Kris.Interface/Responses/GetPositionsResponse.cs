using Kris.Interface.Models;

namespace Kris.Interface.Responses;

public sealed class GetPositionsResponse : Response
{
    public List<UserPositionModel> UserPositions { get; set; }
    public DateTime Resolved { get; set; }
}
