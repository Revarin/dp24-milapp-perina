using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public interface IMessageRepository : IRepository<MessageEntity>
{
    Task<IEnumerable<MessageEntity>> GetByConversationAsync(Guid conversationId, CancellationToken ct);
    Task<IEnumerable<MessageEntity>> GetByConversationAsync(Guid conversationId, int count, CancellationToken ct);
    Task<IEnumerable<MessageEntity>> GetByConversationAsync(Guid conversationId, int count, int from, CancellationToken ct);
}
