using Kris.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data.Repositories;

public sealed class ConversationRepository : RepositoryBase<ConversationEntity>, IConversationRepository
{
    public ConversationRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public async Task<IEnumerable<ConversationEntity>> GetInSessionAsync(Guid sessionId, CancellationToken ct)
    {
        return await _context.Conversations
            .Include(conversation => conversation.Users)
            .Include(conversation => conversation.Messages)
            .Where(conversation => conversation.SessionId == sessionId)
            .ToListAsync();
    }
}
