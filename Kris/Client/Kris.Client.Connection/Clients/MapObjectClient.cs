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

    public async Task<GetOneResponse<Guid>> AddMapPoint(AddMapPointRequest request, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        return await PostAsync<AddMapPointRequest, GetOneResponse<Guid>>(httpClient, "Point", request, ct);
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
