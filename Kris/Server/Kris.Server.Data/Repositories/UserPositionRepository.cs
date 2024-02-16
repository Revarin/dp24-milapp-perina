using Kris.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data.Repositories;

public sealed class UserPositionRepository : RepositoryBase<UserPositionEntity>, IUserPositionRepository
{
    public UserPositionRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public async Task<UserPositionEntity?> GetAsync(Guid userId, Guid sessionId, CancellationToken ct)
    {
        return await _context.UserPositions.FindAsync(userId, sessionId, ct);
    }

    public Task<IEnumerable<UserPositionEntity>> GetWithUsersAsync(Guid sessionId, CancellationToken ct)
    {
        return GetWithUsersAsync(sessionId, DateTime.MinValue, ct);
    }

    public async Task<IEnumerable<UserPositionEntity>> GetWithUsersAsync(Guid sessionId, DateTime from, CancellationToken ct)
    {
        return await _context.UserPositions.Include(position => position.SessionUser)
            .ThenInclude(sessionUser => sessionUser == null ? null : sessionUser.User)
            .Where(position => position.SessionId == sessionId && position.Updated > from)
            .ToListAsync(ct);
    }

    public new async Task<bool> UpdateAsync(UserPositionEntity entity, CancellationToken ct)
    {
        var entityExists = await _context.UserPositions.FindAsync(entity.UserId, entity.SessionId, ct);
        if (entityExists == null) return false;

        _context.UserPositions.Update(entity);
        await _context.SaveChangesAsync(ct);
        return true;
    }
}
