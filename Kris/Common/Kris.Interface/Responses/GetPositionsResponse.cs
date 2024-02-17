using Kris.Interface.Models;

namespace Kris.Interface.Responses;

public sealed class GetPositionsResponse : EmptyResponse
{
    public required List<UserPositionModel> UserPositions { get; set; }
    public required DateTime Resolved { get; set; }
}
