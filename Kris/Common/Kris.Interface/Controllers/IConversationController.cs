using Kris.Interface.Models;
using Kris.Interface.Responses;

namespace Kris.Interface.Controllers;

public interface IConversationController
{
    Task<Response?> DeleteConversation(Guid conversationId, CancellationToken ct);
    Task<GetManyResponse<ConversationListModel>?> GetConversations(CancellationToken ct);
    Task<GetManyResponse<MessageModel>?> GetMessages(Guid conversationId, int? count, int? offset, CancellationToken ct);
}
