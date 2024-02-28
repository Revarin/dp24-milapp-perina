using Kris.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data.Repositories;

public sealed class UserPositionRepository : RepositoryBase<UserPositionEntity>, IUserPositionRepository
{
    public UserPositionRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public Task<IEnumerable<UserPositionEntity>> GetWithUsersAsync(Guid callerId, Guid sessionId, CancellationToken ct)
    {
        return GetWithUsersAsync(callerId, sessionId, DateTime.MinValue, ct);
    }

    public async Task<IEnumerable<UserPositionEntity>> GetWithUsersAsync(Guid callerId, Guid sessionId, DateTime from, CancellationToken ct)
    {
        return await _context.UserPositions.Include(position => position.SessionUser)
            .ThenInclude(sessionUser => sessionUser!.User)
            .Where(position => position.SessionUser!.SessionId == sessionId
                && position.SessionUser!.UserId != callerId
                && position.Updated > from)
            .ToListAsync(ct);
    }
}
