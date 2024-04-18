using Kris.Client.Common.Metrics;
using Kris.Client.Data.Cache;
using Kris.Common.Extensions;
using Kris.Interface.Controllers;
using Kris.Interface.Models;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using System.Web;

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
        SentryMetrics.CounterIncrement("AddMapPoint");
        return await PostAsync<AddMapPointRequest, GetOneResponse<Guid>>(httpClient, "Point", request, ct);
    }

    public async Task<Response> DeleteMapPoint(Guid pointId, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        SentryMetrics.CounterIncrement("DeleteMapPoint");
        return await DeleteAsync<Response>(httpClient, $"Point/{pointId}", ct);
    }

    public async Task<Response> EditMapPoint(EditMapPointRequest request, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        SentryMetrics.CounterIncrement("EditMapPoint");
        return await PutAsync<EditMapPointRequest, Response>(httpClient, "Point", request, ct);
    }

    public async Task<GetMapObjectsResponse> GetMapObjects(DateTime? from, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (from.HasValue) query[nameof(from)] = from.Value.ToISOString();

        SentryMetrics.CounterIncrement("GetMapObjects");
        var result = await GetAsync<GetMapObjectsResponse>(httpClient, string.Empty, query.ToString(), ct);

        if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();
        return result;
    }

    public async Task<GetOneResponse<MapPointDetailModel>> GetMapPoint(Guid pointId, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        SentryMetrics.CounterIncrement("GetMapPoint");
        return await GetAsync<GetOneResponse<MapPointDetailModel>>(httpClient, $"Point/{pointId}", ct);
    }
}
