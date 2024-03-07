using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public interface ISessionRepository : IRepository<SessionEntity>
{
    Task<bool> ExistsAsync(string name, CancellationToken ct);
    Task<SessionEntity?> GetWithAllAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<SessionEntity>> GetWithAllAsync(CancellationToken ct);
    Task<SessionEntity?> GetWithConversations(Guid id, CancellationToken ct);
}
