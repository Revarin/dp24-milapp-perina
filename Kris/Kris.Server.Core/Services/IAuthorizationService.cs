using Kris.Common.Enums;
using Kris.Server.Core.Models;

namespace Kris.Server.Core.Services;

public interface IAuthorizationService
{
    public Task<bool> AuthorizeAsync(CurrentUserModel user, UserType minRole, CancellationToken ct);
    public Task<bool> AuthorizeAsync(CurrentUserModel user, CancellationToken ct);
}
