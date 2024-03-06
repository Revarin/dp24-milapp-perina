using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public interface IConversationRepository : IRepository<ConversationEntity>
{
    Task<ConversationEntity?> GetWithAllAsync(Guid id, CancellationToken ct);
    Task<ConversationEntity?> GetWithUsersAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<ConversationEntity>> GetInSessionAsync(Guid sessionId, CancellationToken ct);
    Task<IEnumerable<ConversationEntity>> GetByUserAsync(Guid userId, Guid sessionId, CancellationToken ct);
}
