﻿using Kris.Common.Extensions;
using Kris.Interface.Controllers;
using Kris.Interface.Models;
using Kris.Interface.Responses;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Requests;
using Kris.Server.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kris.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class ConversationController : KrisController, IConversationController
{
    public ConversationController(IMediator mediator) : base(mediator)
    {
    }

    [HttpDelete("{conversationId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<Response?> DeleteConversation(Guid conversationId, CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<Response>();

        var command = new DeleteConversationCommand { User = user, ConversationId = conversationId };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Unauthorized<Response>(result.Errors.FirstMessage());
            else if (result.HasError<EntityNotFoundError>()) return Response.NotFound<Response>(result.Errors.FirstMessage());
            else if (result.HasError<InvalidOperationError>()) return Response.Forbidden<Response>(result.Errors.FirstMessage());
            else return Response.InternalError<Response>();
        }

        return Response.Ok();
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<GetManyResponse<ConversationListModel>?> GetConversations(CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<GetManyResponse<ConversationListModel>>();

        var query = new GetConversationsQuery { User = user };
        var result = await _mediator.Send(query, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Unauthorized<GetManyResponse<ConversationListModel>>(result.Errors.FirstMessage());
            else return Response.InternalError<GetManyResponse<ConversationListModel>>();
        }

        return Response.Ok(new GetManyResponse<ConversationListModel>(result.Value));
    }

    [HttpGet("{conversationId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<GetManyResponse<MessageModel>?> GetMessages(Guid conversationId, int? count, int? offset, CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<GetManyResponse<MessageModel>>();

        var query = new GetMessagesQuery { User = user, ConversationId = conversationId, Count = count, From = offset };
        var result = await _mediator.Send(query, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Unauthorized<GetManyResponse<MessageModel>>(result.Errors.FirstMessage());
            else if (result.HasError<EntityNotFoundError>()) return Response.NotFound<GetManyResponse<MessageModel>>(result.Errors.FirstMessage());
            else return Response.InternalError<GetManyResponse<MessageModel>>();
        }

        return Response.Ok(new GetManyResponse<MessageModel>(result.Value));
    }
}
