using Kris.Interface.Models;

namespace Kris.Interface.Responses;

public sealed class GetPositionsResponse : ResponseBase
{
    public required List<UserPositionModel> UserPositions { get; set; }
    public required DateTime Updated { get; set; }
}
