using Kris.Interface.Requests;
using Kris.Interface.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Kris.Interface.Controllers;

public interface IPositionController
{
    Task<ActionResult> SavePosition(SavePositionRequest request, CancellationToken ct);
    Task<ActionResult<GetPositionsResponse>> GetPositions(DateTime? from,  CancellationToken ct);
}
