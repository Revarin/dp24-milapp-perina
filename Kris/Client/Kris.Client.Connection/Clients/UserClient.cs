﻿using Kris.Client.Common.Metrics;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Interface.Responses;

namespace Kris.Client.Connection.Clients;

public sealed class UserClient : ClientBase, IUserController
{
    public UserClient(IIdentityStore identityStore, IHttpClientFactory httpClientFactory)
        : base(identityStore, httpClientFactory, "User")
    {
    }

    public async Task<Response> RegisterUser(RegisterUserRequest request, CancellationToken ct)
    {
        using var httpClient = _httpClientFactory.CreateHttpClient(_controller);
        SentryMetrics.CounterIncrement("RegisterUser");
        return await PostAsync<RegisterUserRequest, Response>(httpClient, "Register", request, ct);
    }

    public async Task<LoginResponse> LoginUser(LoginUserRequest request, CancellationToken ct)
    {
        using var httpClient = _httpClientFactory.CreateHttpClient(_controller);
        SentryMetrics.CounterIncrement("LoginUser");
        return await PostAsync<LoginUserRequest, LoginResponse>(httpClient, "Login", request, ct);
    }

    public Task<Response> DeleteUser(PasswordRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<IdentityResponse> EditUser(EditUserRequest request, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        SentryMetrics.CounterIncrement("EditUser");
        return await PutAsync<EditUserRequest, IdentityResponse>(httpClient, "", request, ct);
    }


    public async Task<Response> StoreUserSettings(StoreUserSettingsRequest request, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        SentryMetrics.CounterIncrement("StoreUserSettings");
        return await PostAsync<StoreUserSettingsRequest, Response>(httpClient, "Settings", request, ct);
    }
}
