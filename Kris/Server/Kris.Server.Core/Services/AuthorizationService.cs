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

    public Task<bool> AuthorizeAsync(CurrentUserModel user, CancellationToken ct)
    {
        return AuthorizeAsync(user, UserType.Basic, ct);
    }

    public async Task<bool> AuthorizeAsync(CurrentUserModel user, UserType minRole, CancellationToken ct)
    {
        if (!user.SessionId.HasValue) return false;
        return await _sessionUserRepository.AuthorizeAsync(user.Id, user.SessionId.Value, minRole, ct);
    }
}
