using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Interface.Controllers;
using MediatR;

namespace Kris.Client.Core.Handlers.Conversation;

public sealed class GetConversationsQueryHandler : ConversationHandler, IRequestHandler<GetConversationsQuery, Result<IEnumerable<ConversationListModel>>>
{
    public GetConversationsQueryHandler(IConversationController conversationClient, IConversationMapper conversationMapper)
        : base(conversationClient, conversationMapper)
    {
    }

    public async Task<Result<IEnumerable<ConversationListModel>>> Handle(GetConversationsQuery request, CancellationToken cancellationToken)
    {
        var response = await _conversationClient.GetConversations(cancellationToken);

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else return Result.Fail(new ServerError(response.Message));
        }

        return Result.Ok(response.Values.Select(_conversationMapper.Map));
    }
}
