using Kris.Common.Extensions;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
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
public sealed class MapObjectController : KrisController, IMapObjectController
{
    public MapObjectController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [Authorize]
    public async Task<GetMapObjectsResponse?> GetMapObjects(DateTime? from, CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<GetMapObjectsResponse>();

        var query = new GetMapObjectsQuery { User = user, From = from };
        var result = await _mediator.Send(query, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Unauthorized<GetMapObjectsResponse>(result.Errors.FirstMessage());
            else return Response.BadRequest<GetMapObjectsResponse>();
        }

        return Response.Ok(result.Value);
    }

    [HttpPost("Point")]
    [Authorize]
    public async Task<GetOneResponse<Guid>?> AddMapPoint(AddMapPointRequest request, CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<GetOneResponse<Guid>>();

        var command = new AddMapPointCommand { User = user, AddMapPoint = request };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Unauthorized<GetOneResponse<Guid>>(result.Errors.FirstMessage());
            else return Response.BadRequest<GetOneResponse<Guid>>();
        }

        return Response.Ok(new GetOneResponse<Guid> { Value = result.Value });
    }

    [HttpPut("Point")]
    [Authorize]
    public async Task<Response?> EditMapPoint(EditMapPointRequest request, CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<Response>();

        var command = new EditMapPointCommand { User = user, EditMapPoint = request };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Unauthorized<Response>(result.Errors.FirstMessage());
            else if (result.HasError<EntityNotFoundError>()) return Response.NotFound<Response>(result.Errors.FirstMessage());
            else return Response.BadRequest<Response>();
        }

        return Response.Ok();
    }

    [HttpDelete("Point/{pointId:guid}")]
    [Authorize]
    public async Task<Response?> DeleteMapPoint(Guid pointId, CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<Response>();

        var command = new DeleteMapPointCommand { User = user, MapPointId = pointId };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Unauthorized<Response>(result.Errors.FirstMessage());
            else if (result.HasError<EntityNotFoundError>()) return Response.NotFound<Response>(result.Errors.FirstMessage());
            else return Response.BadRequest<Response>();
        }

        return Response.Ok();
    }
}
