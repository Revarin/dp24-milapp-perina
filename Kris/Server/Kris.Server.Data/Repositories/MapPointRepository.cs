using Kris.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data.Repositories;

public sealed class MapPointRepository : RepositoryBase<MapPointEntity>, IMapPointRepository
{
    public MapPointRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public Task<MapPointEntity?> GetWithAllAsync(Guid id, CancellationToken ct)
    {
        return _context.MapPoints
            .Include(mapPoint => mapPoint.SessionUser)
            .ThenInclude(sessionUser => sessionUser!.User)
            .Include(mapPoint => mapPoint.Attachments)
            .FirstOrDefaultAsync(mapPoint => mapPoint.Id == id, ct);
    }

    public Task<IEnumerable<MapPointEntity>> GetWithUsersAsync(Guid sessionId, CancellationToken ct)
    {
        return GetWithUsersAsync(sessionId, DateTime.MinValue, ct);
    }

    public async Task<IEnumerable<MapPointEntity>> GetWithUsersAsync(Guid sessionId, DateTime from, CancellationToken ct)
    {
        return await _context.MapPoints
            .Include(mapPoint => mapPoint.SessionUser)
            .ThenInclude(sessionUser => sessionUser!.User)
            .Where(mapPoint => mapPoint.SessionUser!.SessionId == sessionId
                && mapPoint.Created > from)
            .ToListAsync(ct);
    }
}
