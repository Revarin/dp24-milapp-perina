using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kris.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class PositionController : KrisController, IPositionController
{
    public PositionController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [Authorize]
    public Task<ActionResult<GetPositionsResponse>> GetPositions(DateTime lastUpdate, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    [Authorize]
    public Task<ActionResult> SavePosition(SavePositionRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
