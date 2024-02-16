using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public interface IMapPointRepository : IRepository<MapPointEntity>
{
    Task<IEnumerable<MapPointEntity>> GetWithUsersAsync(Guid sessionId, CancellationToken ct);
    Task<IEnumerable<MapPointEntity>> GetWithUsersAsync(Guid sessionId, DateTime from, CancellationToken ct);
}
