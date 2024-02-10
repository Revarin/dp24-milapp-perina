using Kris.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data.Repositories;

public sealed class SessionRepository : RepositoryBase<SessionEntity>, ISessionRepository
{
    public SessionRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public async Task<bool> ExistsAsync(string name, CancellationToken ct)
    {
        return await _context.Sessions.AnyAsync(session => session.Name == name, ct);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct)
    {
        var entity = await _context.Sessions.FindAsync(id);
        return entity == null;
    }
}
