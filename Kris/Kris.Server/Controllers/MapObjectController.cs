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
public sealed class MapObjectController : KrisController, IMapObjectController
{
    public MapObjectController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [Authorize]
    public Task<ActionResult<GetMapObjectsResponse>> GetMapObjects(DateTime? from, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    [HttpPost("Point")]
    [Authorize]
    public Task<ActionResult<Guid>> AddMapPoint(AddMapPointRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    [HttpPut("Point")]
    [Authorize]
    public Task<ActionResult> EditMapPoint(EditMapPointRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("Point/{pointId:guid}")]
    [Authorize]
    public Task<ActionResult> DeleteMapPoint(Guid pointId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
