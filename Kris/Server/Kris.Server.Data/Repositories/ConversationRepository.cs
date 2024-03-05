using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public sealed class ConversationRepository : RepositoryBase<ConversationEntity>, IConversationRepository
{
    public ConversationRepository(DataContext dataContext) : base(dataContext)
    {
    }
}
