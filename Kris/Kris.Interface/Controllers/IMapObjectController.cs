using Kris.Interface.Requests;
using Kris.Interface.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Kris.Interface.Controllers;

public interface IMapObjectController
{
    Task<ActionResult<GetMapObjectsResponse>> GetMapObjects(DateTime? from, CancellationToken ct);
    Task<ActionResult<Guid>> AddMapPoint(AddMapPointRequest request, CancellationToken ct);
    Task<ActionResult> EditMapPoint(EditMapPointRequest request, CancellationToken ct);
    Task<ActionResult> DeleteMapPoint(Guid pointId, CancellationToken ct);
}
