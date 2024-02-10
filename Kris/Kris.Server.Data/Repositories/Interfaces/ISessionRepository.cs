using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public interface ISessionRepository : IRepository<SessionEntity>
{
    Task<bool> ExistsAsync(string name, CancellationToken ct);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<SessionEntity>> GetAllAsync(bool onlyActive, CancellationToken ct);
    Task<SessionEntity?> GetByIdAsync(Guid id, CancellationToken ct);
}
