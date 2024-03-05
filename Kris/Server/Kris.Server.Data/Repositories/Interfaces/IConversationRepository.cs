using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public interface IConversationRepository : IRepository<ConversationEntity>
{
    Task<IEnumerable<ConversationEntity>> GetInSessionAsync(Guid sessionId, CancellationToken ct);
}
