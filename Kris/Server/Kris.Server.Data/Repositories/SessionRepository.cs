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

    public async Task<SessionEntity?> GetWithAllAsync(Guid id, CancellationToken ct)
    {
        return await _context.Sessions
            .Include(session => session.Conversations)
            .Include(session => session.Users).ThenInclude(sessionUser => sessionUser.User)
            .FirstOrDefaultAsync(session => session.Id == id, ct);
    }

    public async Task<IEnumerable<SessionEntity>> GetWithAllAsync(CancellationToken ct)
    {
        return await _context.Sessions
            .Include(session => session.Conversations)
            .Include(session => session.Users).ThenInclude(sessionUser => sessionUser.User)
            .ToListAsync(ct);
    }

    public async Task<SessionEntity?> GetWithConversations(Guid id, CancellationToken ct)
    {
        return await _context.Sessions
            .Include(session => session.Conversations).ThenInclude(conversation => conversation.Users)
            .Include(session => session.Conversations).ThenInclude(conversation => conversation.Messages)
            .Include(session => session.Users)
            .FirstOrDefaultAsync(session => session.Id == id, ct);
    }
}
