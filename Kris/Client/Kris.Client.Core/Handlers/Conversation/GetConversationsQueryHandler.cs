using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Metrics;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Common.Enums;
using Kris.Interface.Controllers;
using Kris.Interface.Models;
using Kris.Interface.Responses;
using MediatR;
using System.Net;

namespace Kris.Client.Core.Handlers.Conversation;

public sealed class GetConversationsQueryHandler : ConversationHandler, IRequestHandler<GetConversationsQuery, Result<AvailableConversationsModel>>
{
    private readonly IConversationMapper _conversationMapper;

    public GetConversationsQueryHandler(IConversationMapper conversationMapper, IConversationController conversationClient)
        : base(conversationClient)
    {
        _conversationMapper = conversationMapper;
    }

    public async Task<Result<AvailableConversationsModel>> Handle(GetConversationsQuery request, CancellationToken cancellationToken)
    {
        using var t = SentryMetrics.TimerStart("RequestHandler");
        GetManyResponse<ConversationListModel> response;

        try
        {
            response = await _conversationClient.GetConversations(cancellationToken);
        }
        catch (WebException)
        {
            return Result.Fail(new ConnectionError());
        }

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else return Result.Fail(new ServerError(response.Message));
        }

        var conversations = new AvailableConversationsModel
        {
            SpecialConversations = response.Values.Where(conversation => conversation.ConversationType != ConversationType.Direct)
                .Select(_conversationMapper.Map)
                .OrderByDescending(c => c.LastMessage),
            DirectConversations = response.Values.Where(conversation => conversation.ConversationType == ConversationType.Direct)
                .Select(_conversationMapper.Map)
                .OrderByDescending(c => c.LastMessage)
        };
        return Result.Ok(conversations);
    }
}
