using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Models;
using Kris.Interface.Responses;

namespace Kris.Client.Connection.Clients;

public sealed class ConversationClient : ClientBase, IConversationController
{
    public ConversationClient(IIdentityStore identityStore, IHttpClientFactory httpClientFactory)
        : base(identityStore, httpClientFactory, "Conversation")
    {
    }

    public Task<Response> DeleteConversation(Guid conversationId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<GetManyResponse<ConversationListModel>> GetConversations(CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        return await GetAsync<GetManyResponse<ConversationListModel>>(httpClient, string.Empty, ct);
    }

    public Task<GetManyResponse<MessageModel>> GetMessages(Guid conversationId, int? count, int? offset, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
