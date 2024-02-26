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
        if (user == null) return Response.Unauthorized<IdentityResponse>();

        var command = new EditSessionCommand { EditSession = request, User = user };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Forbidden<IdentityResponse>(result.Errors.FirstMessage());
            else return Response.InternalError<IdentityResponse>();
        }

        return Response.Ok();
    }

    [HttpDelete]
    [AuthorizeRoles(UserType.SuperAdmin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<Response?> EndSession(CancellationToken ct)
    {
        // End CURRENT session only
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<IdentityResponse>();

        var command = new EndSessionCommand() { User = user };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Forbidden<IdentityResponse>(result.Errors.FirstMessage());
            else if (result.HasError<EntityNotFoundError>()) return Response.NotFound<IdentityResponse>(result.Errors.FirstMessage());
            else return Response.InternalError<IdentityResponse>();
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

        var command = new JoinSessionCommand { User = user, JoinSession = request };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<EntityNotFoundError>()) return Response.NotFound<IdentityResponse>(result.Errors.FirstMessage());
            else if (result.HasError<InvalidCredentialsError>()) return Response.Forbidden<IdentityResponse>(result.Errors.FirstMessage());
            else return Response.InternalError<IdentityResponse>();
        }

        return Response.Ok(result.Value);
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

        var command = new LeaveSessionCommand { User = user, SessionId = sessionId };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UserNotInSessionError>()) return Response.BadRequest<IdentityResponse>(result.Errors.FirstMessage());
            if (result.HasError<InvalidOperationError>()) return Response.BadRequest<IdentityResponse>(result.Errors.FirstMessage());
            else return Response.InternalError<IdentityResponse>();
        }

        return Response.Ok(result.Value);
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

        var command = new KickFromSessionCommand { User = user, UserId = userId };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Forbidden<Response>(result.Errors.FirstMessage());
            else if (result.HasError<UserNotInSessionError>()) return Response.NotFound<Response>(result.Errors.FirstMessage());
            else if (result.HasError<InvalidOperationError>()) return Response.BadRequest<Response>(result.Errors.FirstMessage());
            else return Response.InternalError<Response>();
        }

        return Response.Ok();
    }

    [HttpGet("{sessionId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<GetOneResponse<SessionModel>?> GetSession(Guid sessionId, CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<GetOneResponse<SessionModel>>();

        var query = new GetSessionQuery { SessionId = sessionId };
        var result = await _mediator.Send(query, ct);

        if (result.IsFailed)
        {
            if (result.HasError<EntityNotFoundError>()) return Response.NotFound<GetOneResponse<SessionModel>>(result.Errors.FirstMessage());
            else return Response.InternalError<GetOneResponse<SessionModel>>();
        }

        return Response.Ok(new GetOneResponse<SessionModel> { Value = result.Value });
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<GetManyResponse<SessionModel>?> GetSessions(CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<GetManyResponse<SessionModel>>();

        var query = new GetSessionsQuery();
        var result = await _mediator.Send(query, ct);

        if (result.IsFailed) return Response.InternalError<GetManyResponse<SessionModel>>();

        return Response.Ok(new GetManyResponse<SessionModel> { Values = result.Value });
    }
}
