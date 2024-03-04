using Kris.Interface.Requests;
using Kris.Interface.Responses;

namespace Kris.Interface.Controllers;

public interface IMapObjectController
{
    Task<GetMapObjectsResponse?> GetMapObjects(DateTime? from, CancellationToken ct);
    Task<GetOneResponse<Guid>> AddMapPoint(AddMapPointRequest request, CancellationToken ct);
    Task<Response?> EditMapPoint(EditMapPointRequest request, CancellationToken ct);
    Task<Response?> DeleteMapPoint(Guid pointId, CancellationToken ct);
}
