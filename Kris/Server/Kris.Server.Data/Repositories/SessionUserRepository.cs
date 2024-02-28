using Kris.Common.Enums;
using Kris.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data.Repositories;

public sealed class SessionUserRepository : RepositoryBase<SessionUserEntity>, ISessionUserRepository
{
    public SessionUserRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public async Task<SessionUserEntity?> GetAsync(Guid userId, Guid sessionId, CancellationToken ct)
    {
        return await _context.SessionUsers.FirstOrDefaultAsync(su => su.UserId == userId && su.SessionId == sessionId, ct);
    }

    public async Task<bool> AuthorizeAsync(Guid userId, Guid sessionId, UserType minRole, CancellationToken ct)
    {
        var result = await _context.SessionUsers.FirstOrDefaultAsync(su => su.UserId == userId && su.SessionId == sessionId, ct);
        return result?.UserType >= minRole;
    }
}
