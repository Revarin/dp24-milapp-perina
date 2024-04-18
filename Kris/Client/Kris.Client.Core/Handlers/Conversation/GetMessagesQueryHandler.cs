using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Metrics;
using Kris.Client.Common.Options;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Interface.Controllers;
using Kris.Interface.Responses;
using MediatR;
using Microsoft.Extensions.Options;
using System.Net;

using ClientMessageModel = Kris.Client.Core.Models.MessageModel;
using InterfaceMessageModel = Kris.Interface.Models.MessageModel;

namespace Kris.Client.Core.Handlers.Conversation;

public sealed class GetMessagesQueryHandler : ConversationHandler, IRequestHandler<GetMessagesQuery, Result<IEnumerable<ClientMessageModel>>>
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

    public async Task<Result<IEnumerable<ClientMessageModel>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        using var t = SentryMetrics.TimerStart("RequestHandler");
        var from = _settingsOptions.ChatMessagesPageSize * request.Page;
        var count = _settingsOptions.ChatMessagesPageSize;
        GetManyResponse<InterfaceMessageModel> response;

        try
        {
            response = await _conversationClient.GetMessages(request.ConversationId, count, from, cancellationToken);
        }
        catch (WebException)
        {
            return Result.Fail(new ConnectionError());
        }

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else if (response.IsNotFound()) return Result.Fail(new EntityNotFoundError());
            else return Result.Fail(new ServerError(response.Message));
        }

        return Result.Ok(response.Values.Select(_messageMapper.Map));
    }
}
