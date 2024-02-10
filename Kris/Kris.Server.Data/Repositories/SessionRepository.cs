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

    public async Task<IEnumerable<SessionEntity>> GetAllAsync(bool onlyActive, CancellationToken ct)
    {
        return await _context.Sessions.Include(session => session.Users)
            .Where(session => session.IsActive || !onlyActive)
            .ToListAsync(ct);
    }

    public async Task<SessionEntity?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Sessions.Include(session => session.Users)
            .FirstOrDefaultAsync(session => session.Id == id, ct);
    }
}
