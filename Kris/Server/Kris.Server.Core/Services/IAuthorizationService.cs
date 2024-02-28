using Kris.Common.Enums;
using Kris.Server.Core.Models;

namespace Kris.Server.Core.Services;

public interface IAuthorizationService
{
    public Task<AuthorizationResult> AuthorizeAsync(CurrentUserModel user, UserType minRole, CancellationToken ct);
    public Task<AuthorizationResult> AuthorizeAsync(CurrentUserModel user, CancellationToken ct);
}
