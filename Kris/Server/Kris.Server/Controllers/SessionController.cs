using Kris.Common.Enums;
using Kris.Common.Extensions;
using Kris.Interface.Controllers;
using Kris.Interface.Models;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using Kris.Server.Attributes;
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
public sealed class SessionController : KrisController, ISessionController
{
    public SessionController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IdentityResponse?> CreateSession(CreateSessionRequest request, CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<IdentityResponse>();

        var command = new CreateSessionCommand { CreateSession = request, User = user };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<EntityExistsError>()) return Response.BadRequest<IdentityResponse>(result.Errors.FirstMessage());
            else return Response.InternalError<IdentityResponse>();
        }

        return Response.Ok(result.Value);
    }

    [HttpPut]
    [AuthorizeRoles(UserType.Admin, UserType.SuperAdmin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<Response?> EditSession(EditSessionRequest request, CancellationToken ct)
    {
        // Edit CURRENT session only
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<Response>();

        var command = new EditSessionCommand { EditSession = request, User = user };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Unauthorized<Response>(result.Errors.FirstMessage());
            else if (result.HasError<InvalidCredentialsError>()) return Response.Forbidden<Response>(result.Errors.FirstMessage());
            else return Response.InternalError<Response>();
        }

        return Response.Ok();
    }

    [HttpPut("User")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IdentityResponse?> EditSessionUser(EditSessionUserRequest request, CancellationToken ct)
    {
        // Edit in CURRENT session only
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<IdentityResponse>();

        var command = new EditSessionUserCommand { EditSessionUser = request, User = user };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Unauthorized<IdentityResponse>(result.Errors.FirstMessage());
            else return Response.InternalError<IdentityResponse>();
        }

        return Response.Ok(result.Value);
    }

    [HttpDelete]
    [AuthorizeRoles(UserType.SuperAdmin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<Response?> EndSession(PasswordRequest request, CancellationToken ct)
    {
        // End CURRENT session only
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<Response>();

        var command = new EndSessionCommand() { User = user, Password = request.Password };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Unauthorized<Response>(result.Errors.FirstMessage());
            else if (result.HasError<InvalidCredentialsError>()) return Response.Forbidden<Response>(result.Errors.FirstMessage());
            else if (result.HasError<EntityNotFoundError>()) return Response.NotFound<Response>(result.Errors.FirstMessage());
            else return Response.InternalError<Response>();
        }

        return Response.Ok();
    }

    [HttpPut("Join")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IdentityResponse?> JoinSession(JoinSessionRequest request, CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<IdentityResponse>();

        var joinCommand = new JoinSessionCommand { User = user, JoinSession = request };
        var joinResult = await _mediator.Send(joinCommand, ct);

        if (joinResult.IsFailed)
        {
            if (joinResult.HasError<EntityNotFoundError>()) return Response.NotFound<IdentityResponse>(joinResult.Errors.FirstMessage());
            else if (joinResult.HasError<InvalidCredentialsError>()) return Response.Forbidden<IdentityResponse>(joinResult.Errors.FirstMessage());
            else return Response.InternalError<IdentityResponse>();
        }

        var conversationCommand = new JoinConversationsCommand { UserId = user.UserId, SessionId = request.Id };
        var conversationResult = await _mediator.Send(conversationCommand, ct);

        if (conversationResult.IsFailed)
        {
            if (joinResult.HasError<EntityNotFoundError>()) return Response.NotFound<IdentityResponse>(joinResult.Errors.FirstMessage());
            else return Response.InternalError<IdentityResponse>();
        }

        return Response.Ok(joinResult.Value);
    }

    [HttpPut("Leave/{sessionId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IdentityResponse?> LeaveSession(Guid sessionId, CancellationToken ct)
    {
        // For users, leave given session
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<IdentityResponse>();

        var leaveCommand = new LeaveSessionCommand { User = user, SessionId = sessionId };
        var leaveResult = await _mediator.Send(leaveCommand, ct);

        if (leaveResult.IsFailed)
        {
            if (leaveResult.HasError<UserNotInSessionError>()) return Response.BadRequest<IdentityResponse>(leaveResult.Errors.FirstMessage());
            if (leaveResult.HasError<InvalidOperationError>()) return Response.BadRequest<IdentityResponse>(leaveResult.Errors.FirstMessage());
            else return Response.InternalError<IdentityResponse>();
        }

        var conversationCommand = new RemoveEmptyConversationsCommand { SessionId = sessionId };
        var conversationResult = await _mediator.Send(conversationCommand, ct);

        if (conversationResult.IsFailed)
        {
            return Response.InternalError<IdentityResponse>();
        }

        return Response.Ok(leaveResult.Value);
    }

    [HttpPut("Kick/{userId:guid}")]
    [AuthorizeRoles(UserType.Admin, UserType.SuperAdmin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<Response?> KickFromSession(Guid userId, CancellationToken ct)
    {
        // For admins, kick user from CURRENT session
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<Response>();

        var kickCommand = new KickFromSessionCommand { User = user, UserId = userId };
        var kickResult = await _mediator.Send(kickCommand, ct);

        if (kickResult.IsFailed)
        {
            if (kickResult.HasError<UnauthorizedError>()) return Response.Forbidden<Response>(kickResult.Errors.FirstMessage());
            else if (kickResult.HasError<UserNotInSessionError>()) return Response.NotFound<Response>(kickResult.Errors.FirstMessage());
            else if (kickResult.HasError<InvalidOperationError>()) return Response.BadRequest<Response>(kickResult.Errors.FirstMessage());
            else return Response.InternalError<Response>();
        }

        var conversationCommand = new RemoveEmptyConversationsCommand { SessionId = user.SessionId!.Value };
        var conversationResult = await _mediator.Send(conversationCommand, ct);

        if (conversationResult.IsFailed)
        {
            return Response.InternalError<IdentityResponse>();
        }

        return Response.Ok();
    }

    [HttpGet("{sessionId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<GetOneResponse<SessionDetailModel>?> GetSession(Guid sessionId, CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<GetOneResponse<SessionDetailModel>>();

        var query = new GetSessionQuery { User = user, SessionId = sessionId };
        var result = await _mediator.Send(query, ct);

        if (result.IsFailed)
        {
            if (result.HasError<EntityNotFoundError>()) return Response.NotFound<GetOneResponse<SessionDetailModel>>(result.Errors.FirstMessage());
            else return Response.InternalError<GetOneResponse<SessionDetailModel>>();
        }

        return Response.Ok(new GetOneResponse<SessionDetailModel> { Value = result.Value });
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<GetManyResponse<SessionListModel>?> GetSessions(CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<GetManyResponse<SessionListModel>>();

        var query = new GetSessionsQuery();
        var result = await _mediator.Send(query, ct);

        if (result.IsFailed) return Response.InternalError<GetManyResponse<SessionListModel>>();

        return Response.Ok(new GetManyResponse<SessionListModel>(result.Value));
    }
}
