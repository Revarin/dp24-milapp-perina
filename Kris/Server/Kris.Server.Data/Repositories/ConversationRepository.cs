using Kris.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data.Repositories;

public sealed class ConversationRepository : RepositoryBase<ConversationEntity>, IConversationRepository
{
    public ConversationRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public async Task<IEnumerable<ConversationEntity>> GetByUserAsync(Guid userId, Guid sessionId, CancellationToken ct)
    {
        return await _context.Conversations
            .Include(conversation => conversation.Messages.OrderByDescending(message => message.TimeStamp))
            .Include(conversation => conversation.Users).ThenInclude(sessionUser => sessionUser.User)
            .Where(conversation => conversation.SessionId == sessionId
                && conversation.Users.Any(user => user.UserId == userId))
            .ToListAsync();
    }

    public async Task<IEnumerable<ConversationEntity>> GetInSessionAsync(Guid sessionId, CancellationToken ct)
    {
        return await _context.Conversations
            .Include(conversation => conversation.Users)
            .Include(conversation => conversation.Messages)
            .Where(conversation => conversation.SessionId == sessionId)
            .ToListAsync();
    }

    public async Task<ConversationEntity?> GetWithAllAsync(Guid id, CancellationToken ct)
    {
        return await _context.Conversations
            .Include(conversation => conversation.Users)
            .Include(conversation => conversation.Messages)
            .FirstOrDefaultAsync(conversation => conversation.Id == id, ct);
    }

    public async Task<ConversationEntity?> GetWithUsersAsync(Guid id, CancellationToken ct)
    {
        return await _context.Conversations
            .Include(conversation => conversation.Users)
            .FirstOrDefaultAsync(conversation => conversation.Id == id, ct);
    }
}
