using Kris.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data.Repositories;

public sealed class SessionRepository : RepositoryBase<SessionEntity>, ISessionRepository
{
    public SessionRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public async Task<bool> SessionExistsAsync(string name, CancellationToken ct)
    {
        return await _context.Sessions.AnyAsync(session => session.Name == name, ct);
    }
}
