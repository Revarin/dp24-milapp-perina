using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Interface.Responses;

namespace Kris.Client.Connection.Clients;

public sealed class UserClient : ClientBase, IUserController
{
    public UserClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory, "User")
    {
    }

    public async Task<Response<EmptyResponse>> RegisterUser(RegisterUserRequest request, CancellationToken ct)
    {
        using var client = _httpClientFactory.CreateHttpClient(_controller);
        return await PostAsync<RegisterUserRequest, Response<EmptyResponse>>(client, "Register", request, ct);
    }

    public Task<Response<JwtTokenResponse>> LoginUser(LoginUserRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<Response<EmptyResponse>> DeleteUser(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<Response<JwtTokenResponse>> EditUser(EditUserRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }


    public Task<Response<object>> StoreUserSettings(StoreUserSettingsRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
