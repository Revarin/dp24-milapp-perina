using Kris.Client.Common.Options;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using Microsoft.Extensions.Options;

namespace Kris.Client.Connection.Clients;

public sealed class UserClient : ClientBase, IUserController
{
    public UserClient(IOptions<SettingsOptions> options) : base(options, "User")
    {
    }

    public async Task<Response<EmptyResponse>> RegisterUser(RegisterUserRequest request, CancellationToken ct)
    {
        return await PostAsync<RegisterUserRequest, Response<EmptyResponse>>("Register", request, ct);
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
