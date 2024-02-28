using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Interface.Responses;

namespace Kris.Client.Connection.Clients;

public sealed class MapObjectClient : ClientBase, IMapObjectController
{
    public MapObjectClient(IIdentityStore identityStore, IHttpClientFactory httpClientFactory)
        : base(identityStore, httpClientFactory, "MapObject")
    {
    }

    public Task<GetOneResponse<Guid>> AddMapPoint(AddMapPointRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<Response> DeleteMapPoint(Guid pointId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<Response> EditMapPoint(EditMapPointRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<GetMapObjectsResponse> GetMapObjects(DateTime? from, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
