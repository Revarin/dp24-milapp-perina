﻿using Kris.Client.Common.Metrics;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Models;
using Kris.Interface.Responses;
using System.Web;

namespace Kris.Client.Connection.Clients;

public sealed class ConversationClient : ClientBase, IConversationController
{
    public ConversationClient(IIdentityStore identityStore, IHttpClientFactory httpClientFactory)
        : base(identityStore, httpClientFactory, "Conversation")
    {
    }

    public async Task<Response> DeleteConversation(Guid conversationId, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        SentryMetrics.CounterIncrement("DeleteConversation");
        return await DeleteAsync<Response>(httpClient, conversationId.ToString(), ct);
    }

    public async Task<GetManyResponse<ConversationListModel>> GetConversations(CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);
        SentryMetrics.CounterIncrement("GetConversations");
        return await GetAsync<GetManyResponse<ConversationListModel>>(httpClient, string.Empty, ct);
    }

    public async Task<GetManyResponse<MessageModel>> GetMessages(Guid conversationId, int? count, int? offset, CancellationToken ct)
    {
        var jwt = _identityStore.GetJwtToken();
        using var httpClient = _httpClientFactory.CreateAuthentizedHttpClient(_controller, jwt);

        var query = HttpUtility.ParseQueryString(string.Empty);
        if (count.HasValue) query[nameof(count)] = count.Value.ToString();
        if (offset.HasValue) query[nameof(offset)] = offset.Value.ToString();

        SentryMetrics.CounterIncrement("GetMessages");
        return await GetAsync<GetManyResponse<MessageModel>>(httpClient, conversationId.ToString(), query.ToString(), ct);
    }
}
