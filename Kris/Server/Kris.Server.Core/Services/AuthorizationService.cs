using Kris.Common.Enums;
using Kris.Server.Core.Models;
using Kris.Server.Data.Repositories;

namespace Kris.Server.Core.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly ISessionUserRepository _sessionUserRepository;

    public AuthorizationService(ISessionUserRepository sessionUserRepository)
    {
        _sessionUserRepository = sessionUserRepository;
    }

    public Task<AuthorizationResult> AuthorizeAsync(CurrentUserModel user, CancellationToken ct)
    {
        return AuthorizeAsync(user, UserType.Basic, ct);
    }

    public async Task<AuthorizationResult> AuthorizeAsync(CurrentUserModel user, UserType minRole, CancellationToken ct)
    {
        if (!user.SessionId.HasValue) return new AuthorizationResult { IsAuthorized = false };

        var sessionUser = await _sessionUserRepository.GetAsync(user.UserId, user.SessionId.Value, ct);
        if (sessionUser == null) return new AuthorizationResult { IsAuthorized = false };
        if (sessionUser.UserType < minRole) return new AuthorizationResult { IsAuthorized = false };

        return new AuthorizationResult { IsAuthorized = true, SessionUserId = sessionUser.Id, UserType = sessionUser.UserType };
    }
}
