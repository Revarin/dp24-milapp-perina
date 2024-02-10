using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public interface ISessionRepository : IRepository<SessionEntity>
{
    Task<bool> ExistsAsync(string name, CancellationToken ct);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct);
}
