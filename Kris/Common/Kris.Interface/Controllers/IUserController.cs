using Kris.Interface.Requests;
using Kris.Interface.Responses;

namespace Kris.Interface.Controllers;

public interface IUserController
{
    Task<Response?> RegisterUser(RegisterUserRequest request, CancellationToken ct);
    Task<LoginResponse?> LoginUser(LoginUserRequest request, CancellationToken ct);
    Task<IdentityResponse?> EditUser(EditUserRequest request, CancellationToken ct);
    Task<Response?> DeleteUser(CancellationToken ct);
    Task<Response?> StoreUserSettings(StoreUserSettingsRequest request, CancellationToken ct);
}
