using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public interface ISessionRepository : IRepository<SessionEntity>
{
    Task<bool> ExistsAsync(string name, CancellationToken ct);
    Task<SessionEntity?> GetWithUsersAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<SessionEntity>> GetWithUsersAsync(CancellationToken ct);
}
