using Kris.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data.Repositories;

public sealed class MapPointRepository : RepositoryBase<MapPointEntity>, IMapPointRepository
{
    public MapPointRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public Task<IEnumerable<MapPointEntity>> GetWithUsersAsync(Guid sessionId, CancellationToken ct)
    {
        return GetWithUsersAsync(sessionId, DateTime.MinValue, ct);
    }

    public async Task<IEnumerable<MapPointEntity>> GetWithUsersAsync(Guid sessionId, DateTime from, CancellationToken ct)
    {
        return await _context.MapPoints.Include(mapPoint => mapPoint.SessionUser)
            .ThenInclude(sessionUser => sessionUser == null ? null : sessionUser.User)
            .Where(mapPoint => mapPoint.SessionId == sessionId && mapPoint.Created > from)
            .ToListAsync(ct);
    }
}
