using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Models;
using Kris.Interface.Requests;
using Kris.Interface.Responses;

namespace Kris.Client.Connection.Clients;

public sealed class SessionClient : ClientBase, ISessionController
{
    public SessionClient(IIdentityStore identityStore, IHttpClientFactory httpClientFactory)
        : base(identityStore, httpClientFactory, "Session")
    {
    }

    public async Task<IdentityResponse> CreateSession(CreateSessionRequest request, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        return await PostAsync<CreateSessionRequest, IdentityResponse>(httpClient, string.Empty, request, ct);
    }

    public async Task<Response> EditSession(EditSessionRequest request, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        return await PutAsync<EditSessionRequest, Response>(httpClient, string.Empty, request, ct);
    }

    public Task<IdentityResponse> EditSessionUser(EditSessionUserRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<Response> EndSession(PasswordRequest request, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        return await DeleteAsync<PasswordRequest, Response>(httpClient, string.Empty, request, ct);
    }

    public async Task<GetOneResponse<SessionModel>> GetSession(Guid sessionId, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        return await GetAsync<GetOneResponse<SessionModel>>(httpClient, sessionId.ToString(), ct);
    }

    public async Task<GetManyResponse<SessionModel>> GetSessions(CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        return await GetAsync<GetManyResponse<SessionModel>>(httpClient, string.Empty, ct);
    }

    public async Task<IdentityResponse> JoinSession(JoinSessionRequest request, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        return await PutAsync<JoinSessionRequest, IdentityResponse>(httpClient, "Join", request, ct);
    }

    public Task<Response> KickFromSession(Guid userId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<IdentityResponse> LeaveSession(Guid sessionId, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        return await PutAsync<IdentityResponse>(httpClient, $"Leave/{sessionId}", ct);
    }
}
