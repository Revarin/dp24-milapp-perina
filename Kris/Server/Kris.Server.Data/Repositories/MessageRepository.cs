using Kris.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data.Repositories;

public sealed class MessageRepository : RepositoryBase<MessageEntity>, IMessageRepository
{
    public MessageRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public async Task<IEnumerable<MessageEntity>> GetByConversationAsync(Guid conversationId, CancellationToken ct)
    {
        return await _context.Messages
            .Include(message => message.Sender)
            .ThenInclude(sessionUser => sessionUser!.User)
            .Where(message => message.ConversationId == conversationId)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<MessageEntity>> GetByConversationAsync(Guid conversationId, int count, CancellationToken ct)
    {
        return await _context.Messages
            .Include(message => message.Sender)
            .ThenInclude(sessionUser => sessionUser!.User)
            .Where(message => message.ConversationId == conversationId)
            .OrderByDescending(message => message.TimeStamp)
            .Take(count)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<MessageEntity>> GetByConversationAsync(Guid conversationId, int count, int from, CancellationToken ct)
    {
        return await _context.Messages
            .Include(message => message.Sender)
            .ThenInclude(sessionUser => sessionUser!.User)
            .Where(message => message.ConversationId == conversationId)
            .OrderByDescending(message => message.TimeStamp)
            .Skip(from)
            .Take(count)
            .ToListAsync(ct);
    }
}
