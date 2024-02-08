using Kris.Interface.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Kris.Interface.Interfaces;

public interface IUserController
{
    Task<ActionResult> RegisterUser(RegisterUserRequest request, CancellationToken ct);
    Task<ActionResult<string>> LoginUser(LoginUserRequest request, CancellationToken ct);
    Task<ActionResult<object>> StoreUserSettings(StoreUserSettingsRequest request);
}
