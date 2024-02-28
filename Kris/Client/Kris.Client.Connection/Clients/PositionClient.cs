using Kris.Client.Data.Cache;
using Kris.Common.Extensions;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using System.Web;

namespace Kris.Client.Connection.Clients;

public sealed class PositionClient : ClientBase, IPositionController
{
    public PositionClient(IIdentityStore identityStore, IHttpClientFactory httpClientFactory)
        : base(identityStore, httpClientFactory, "Position")
    {
    }

    public async Task<GetPositionsResponse> GetPositions(DateTime? from, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (from.HasValue) query[nameof(from)] = from.Value.ToISOString();
        var result = await GetAsync<GetPositionsResponse>(httpClient, string.Empty, query.ToString(), ct);

        if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();
        return result;
    }

    public async Task<Response> SavePosition(SavePositionRequest request, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        var result = await PostAsync<SavePositionRequest, Response>(httpClient, string.Empty, request, ct);

        if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();
        return result;
    }
}
