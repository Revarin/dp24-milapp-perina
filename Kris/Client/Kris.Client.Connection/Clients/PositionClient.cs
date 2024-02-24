using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Interface.Responses;

namespace Kris.Client.Connection.Clients;

public sealed class PositionClient : ClientBase, IPositionController
{
    public PositionClient(IIdentityStore identityStore, IHttpClientFactory httpClientFactory)
        : base(identityStore, httpClientFactory, "Position")
    {
    }

    public Task<GetPositionsResponse> GetPositions(DateTime? from, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<Response> SavePosition(SavePositionRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
