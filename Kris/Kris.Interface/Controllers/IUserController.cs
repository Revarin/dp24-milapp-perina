using Kris.Interface.Requests;
using Kris.Interface.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Kris.Interface.Controllers;

public interface IUserController
{
    Task<ActionResult> RegisterUser(RegisterUserRequest request, CancellationToken ct);
    Task<ActionResult<JwtTokenResponse>> LoginUser(LoginUserRequest request, CancellationToken ct);
    Task<ActionResult<object>> StoreUserSettings(StoreUserSettingsRequest request);
}
