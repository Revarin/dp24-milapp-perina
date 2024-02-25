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
        using var client = _httpClientFactory.CreateHttpClient(_controller);
        return await PostAsync<RegisterUserRequest, Response>(client, "Register", request, ct);
    }

    public async Task<LoginResponse> LoginUser(LoginUserRequest request, CancellationToken ct)
    {
        using var client = _httpClientFactory.CreateHttpClient(_controller);
        return await PostAsync<LoginUserRequest, LoginResponse>(client, "Login", request, ct);
    }

    public Task<Response> DeleteUser(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<LoginResponse> EditUser(EditUserRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }


    public Task<Response> StoreUserSettings(StoreUserSettingsRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
