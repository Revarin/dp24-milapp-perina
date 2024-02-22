﻿using Kris.Client.Data.Cache;
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

    public async Task<LoginResponse> CreateSession(CreateSessionRequest request, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        return await PostAsync<CreateSessionRequest, LoginResponse>(httpClient, "", request, ct);
    }

    public Task<Response> EditSession(EditSessionRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<Response> EndSession(CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        return await DeleteAsync<Response>(httpClient, "", ct);
    }

    public Task<GetOneResponse<SessionModel>> GetSession(Guid sessionId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<GetManyResponse<SessionModel>> GetSessions(CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var client = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        return await GetAsync<GetManyResponse<SessionModel>>(client, "", ct);
    }

    public async Task<LoginResponse> JoinSession(JoinSessionRequest request, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var client = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        return await PutAsync<JoinSessionRequest, LoginResponse>(client, "Join", request, ct);
    }

    public Task<Response> KickFromSession(Guid userId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<LoginResponse> LeaveSession(Guid sessionId, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var client = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        return await PutAsync<LoginResponse>(client, $"Leave/{sessionId}", ct);
    }
}
