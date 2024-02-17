using Kris.Interface.Requests;
using Kris.Interface.Responses;

namespace Kris.Interface.Controllers;

public interface IPositionController
{
    Task<Response<EmptyResponse>> SavePosition(SavePositionRequest request, CancellationToken ct);
    Task<Response<GetPositionsResponse>> GetPositions(DateTime? from,  CancellationToken ct);
}
