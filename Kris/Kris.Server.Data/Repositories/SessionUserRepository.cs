using Kris.Common.Enums;
using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public sealed class SessionUserRepository : RepositoryBase<SessionUserEntity>, ISessionUserRepository
{
    public SessionUserRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public async Task<bool> AuthorizeAsync(Guid userId, Guid sessionId, UserType minRole, CancellationToken ct)
    {
        var result = await _context.SessionUsers.FindAsync(userId, sessionId, ct);
        return result?.UserType >= minRole;
    }
}
