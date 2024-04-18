using Kris.Client.Common.Metrics;
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
        SentryMetrics.CounterIncrement("CreateSession");
        return await PostAsync<CreateSessionRequest, IdentityResponse>(httpClient, string.Empty, request, ct);
    }

    public async Task<Response> EditSession(EditSessionRequest request, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        SentryMetrics.CounterIncrement("EditSession");
        return await PutAsync<EditSessionRequest, Response>(httpClient, string.Empty, request, ct);
    }

    public async Task<IdentityResponse> EditSessionUser(EditSessionUserRequest request, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        SentryMetrics.CounterIncrement("EditSessionUser");
        return await PutAsync<EditSessionUserRequest, IdentityResponse>(httpClient, "User", request, ct);
    }

    public async Task<Response> EditSessionUserRole(EditSessionUserRoleRequest request, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        SentryMetrics.CounterIncrement("EditSessionUserRole");
        return await PutAsync<EditSessionUserRoleRequest, Response>(httpClient, "Role", request, ct);
    }

    public async Task<Response> EndSession(PasswordRequest request, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        SentryMetrics.CounterIncrement("EndSession");
        return await DeleteAsync<PasswordRequest, Response>(httpClient, string.Empty, request, ct);
    }

    public async Task<GetOneResponse<SessionDetailModel>> GetSession(Guid sessionId, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        SentryMetrics.CounterIncrement("GetSession");
        return await GetAsync<GetOneResponse<SessionDetailModel>>(httpClient, sessionId.ToString(), ct);
    }

    public async Task<GetManyResponse<SessionListModel>> GetSessions(CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        SentryMetrics.CounterIncrement("GetSessions");
        return await GetAsync<GetManyResponse<SessionListModel>>(httpClient, string.Empty, ct);
    }

    public async Task<IdentityResponse> JoinSession(JoinSessionRequest request, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        SentryMetrics.CounterIncrement("JoinSession");
        return await PutAsync<JoinSessionRequest, IdentityResponse>(httpClient, "Join", request, ct);
    }

    public async Task<Response> KickFromSession(Guid userId, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        SentryMetrics.CounterIncrement("KickFromSession");
        return await PutAsync<Response>(httpClient, $"Kick/{userId}", ct);
    }

    public async Task<IdentityResponse> LeaveSession(Guid sessionId, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        SentryMetrics.CounterIncrement("LeaveSession");
        return await PutAsync<IdentityResponse>(httpClient, $"Leave/{sessionId}", ct);
    }
}
