using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using Kris.Common.Extensions;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Requests;
using Kris.Server.Extensions;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<GetPositionsResponse?> GetPositions(DateTime? from, CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<GetPositionsResponse>();

        var query = new GetPositionsQuery { User = user, From = from };
        var result = await _mediator.Send(query, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Forbidden<GetPositionsResponse>(result.Errors.FirstMessage());
            else return Response.InternalError<GetPositionsResponse>();
        }

        return Response.Ok(result.Value);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<Response?> SavePosition(SavePositionRequest request, CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<Response>();

        var command = new SavePositionCommand { User = user, SavePosition = request };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Forbidden<Response>(result.Errors.FirstMessage());
            else return Response.InternalError<Response>();
        }

        return Response.Ok();
    }
}
