using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kris.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class MapObjectController : KrisController, IMapObjectController
{
    public MapObjectController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<GetMapObjectsResponse>> GetMapObjects(DateTime? from, CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Unauthorized();

        var query = new GetMapObjectsQuery { User = user, From = from };
        var result = await _mediator.Send(query, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Unauthorized(result.Errors.Select(e => e.Message));
            else return BadRequest();
        }

        return Ok(result.Value);
    }

    [HttpPost("Point")]
    [Authorize]
    public async Task<ActionResult<Guid>> AddMapPoint(AddMapPointRequest request, CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Unauthorized();

        var command = new AddMapPointCommand { User = user, AddMapPoint = request };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Unauthorized(result.Errors.Select(e => e.Message));
            else return BadRequest();
        }

        return Ok(result.Value);
    }

    [HttpPut("Point")]
    [Authorize]
    public async Task<ActionResult> EditMapPoint(EditMapPointRequest request, CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Unauthorized();

        var command = new EditMapPointCommand { User = user, EditMapPoint = request };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Unauthorized(result.Errors.Select(e => e.Message));
            else if (result.HasError<EntityNotFoundError>()) return NotFound(result.Errors.Select(e => e.Message));
            else return BadRequest();
        }

        return Ok();
    }

    [HttpDelete("Point/{pointId:guid}")]
    [Authorize]
    public async Task<ActionResult> DeleteMapPoint(Guid pointId, CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Unauthorized();

        var command = new DeleteMapPointCommand { User = user, MapPointId = pointId };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Unauthorized(result.Errors.Select(e => e.Message));
            else if (result.HasError<EntityNotFoundError>()) return NotFound(result.Errors.Select(e => e.Message));
            else return BadRequest();
        }

        return Ok();
        throw new NotImplementedException();
    }
}
