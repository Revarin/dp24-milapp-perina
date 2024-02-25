using Kris.Interface.Requests;
using Kris.Interface.Responses;

namespace Kris.Interface.Controllers;

public interface IPositionController
{
    Task<Response?> SavePosition(SavePositionRequest request, CancellationToken ct);
    Task<GetPositionsResponse?> GetPositions(DateTime? from,  CancellationToken ct);
}
