using Kris.Interface.Requests;
using Kris.Interface.Responses;

namespace Kris.Interface.Controllers;

public interface IUserController
{
    Task<Response<EmptyResponse>> RegisterUser(RegisterUserRequest request, CancellationToken ct);
    Task<Response<JwtTokenResponse>> LoginUser(LoginUserRequest request, CancellationToken ct);
    Task<Response<JwtTokenResponse>> EditUser(EditUserRequest request, CancellationToken ct);
    Task<Response<EmptyResponse>> DeleteUser(CancellationToken ct);
    Task<Response<object>> StoreUserSettings(StoreUserSettingsRequest request, CancellationToken ct);
}
