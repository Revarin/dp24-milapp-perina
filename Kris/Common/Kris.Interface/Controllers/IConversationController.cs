using Kris.Interface.Responses;

namespace Kris.Interface.Controllers;

public interface IConversationController
{
    Task<Response?> DeleteConversation(Guid conversationId, CancellationToken ct);
}
