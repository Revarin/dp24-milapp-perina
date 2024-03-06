using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Options;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Interface.Controllers;
using MediatR;
using Microsoft.Extensions.Options;

namespace Kris.Client.Core.Handlers.Conversation;

public sealed class GetMessagesQueryHandler : ConversationHandler, IRequestHandler<GetMessagesQuery, Result<IEnumerable<MessageModel>>>
{
    private readonly SettingsOptions _settingsOptions;
    private readonly IMessageMapper _messageMapper;

    public GetMessagesQueryHandler(IOptions<SettingsOptions> options, IMessageMapper messageMapper,
        IConversationController conversationClient)
        : base(conversationClient)
    {
        _settingsOptions = options.Value;
        _messageMapper = messageMapper;
    }

    public async Task<Result<IEnumerable<MessageModel>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var from = _settingsOptions.ChatMessagesPageSize * request.Page;
        var count = _settingsOptions.ChatMessagesPageSize;
        var response = await _conversationClient.GetMessages(request.ConversationId, count, from, cancellationToken);

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else if (response.IsNotFound()) return Result.Fail(new EntityNotFoundError());
            else return Result.Fail(new ServerError(response.Message));
        }

        return Result.Ok(response.Values.Select(_messageMapper.Map));
    }
}
