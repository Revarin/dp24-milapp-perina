using Kris.Common.Enums;
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
    public async Task<Response<JwtTokenResponse>> CreateSession(CreateSessionRequest request, CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<JwtTokenResponse>();

        var command = new CreateSessionCommand { CreateSession = request, User = user };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<EntityExistsError>()) return Response.BadRequest<JwtTokenResponse>(result.Errors.FirstMessage());
            else return Response.InternalError<JwtTokenResponse>();
        }

        return Response.Ok(new JwtTokenResponse { Token = result.Value.Token});
    }

    [HttpPut]
    [AuthorizeRoles(UserType.Admin, UserType.SuperAdmin)]
    public async Task<Response<JwtTokenResponse>> EditSession(EditSessionRequest request, CancellationToken ct)
    {
        // Edit CURRENT session only
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<JwtTokenResponse>();

        var command = new EditSessionCommand { EditSession = request, User = user };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Unauthorized<JwtTokenResponse>(result.Errors.FirstMessage());
            else return Response.InternalError<JwtTokenResponse>();
        }

        return Response.Ok(new JwtTokenResponse { Token = result.Value.Token });
    }

    [HttpDelete]
    [AuthorizeRoles(UserType.SuperAdmin)]
    public async Task<Response<JwtTokenResponse>> EndSession(CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<JwtTokenResponse>();

        var command = new EndSessionCommand() { User = user };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Unauthorized<JwtTokenResponse>(result.Errors.FirstMessage());
            else if (result.HasError<EntityNotFoundError>()) return Response.NotFound<JwtTokenResponse>(result.Errors.FirstMessage());
            else return Response.InternalError<JwtTokenResponse>();
        }

        return Response.Ok(new JwtTokenResponse { Token = result.Value.Token });
    }

    [HttpPut("Join")]
    [Authorize]
    public async Task<Response<JwtTokenResponse>> JoinSession(JoinSessionRequest request, CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<JwtTokenResponse>();

        var command = new JoinSessionCommand { User = user, JoinSession = request };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<EntityNotFoundError>()) return Response.NotFound<JwtTokenResponse>(result.Errors.FirstMessage());
            else if (result.HasError<InvalidCredentialsError>()) return Response.Unauthorized<JwtTokenResponse>(result.Errors.FirstMessage());
            else return Response.InternalError<JwtTokenResponse>();
        }

        return Response.Ok(new JwtTokenResponse { Token = result.Value.Token });
    }

    [HttpPut("Leave/{sessionId:guid}")]
    [Authorize]
    public async Task<Response<JwtTokenResponse>> LeaveSession(Guid sessionId, CancellationToken ct)
    {
        // For users, leave given session
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<JwtTokenResponse>();

        var command = new LeaveSessionCommand { User = user, SessionId = sessionId };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UserNotInSessionError>()) return Response.BadRequest<JwtTokenResponse>(result.Errors.FirstMessage());
            else return Response.InternalError<JwtTokenResponse>();
        }

        return Response.Ok(new JwtTokenResponse { Token = result.Value.Token });
    }

    [HttpPut("Kick/{userId:guid}")]
    [AuthorizeRoles(UserType.Admin, UserType.SuperAdmin)]
    public async Task<Response<EmptyResponse>> KickFromSession(Guid userId, CancellationToken ct)
    {
        // For admins, kick user from CURRENT session
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<EmptyResponse>();

        var command = new KickFromSessionCommand { User = user, UserId = userId };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Unauthorized<EmptyResponse>(result.Errors.FirstMessage());
            else if (result.HasError<UserNotInSessionError>()) return Response.NotFound<EmptyResponse>(result.Errors.FirstMessage());
            else if (result.HasError<InvalidOperationError>()) return Response.BadRequest<EmptyResponse>(result.Errors.FirstMessage());
            else return Response.InternalError<EmptyResponse>();
        }

        return Response.Ok<EmptyResponse>();
    }

    [HttpGet("{sessionId:guid}")]
    [Authorize]
    public async Task<Response<GetOneResponse<SessionModel>>> GetSession(Guid sessionId, CancellationToken ct)
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
    public async Task<Response<GetManyResponse<SessionModel>>> GetSessions(CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<GetManyResponse<SessionModel>>();

        var query = new GetSessionsQuery();
        var result = await _mediator.Send(query, ct);

        if (result.IsFailed) return Response.InternalError<GetManyResponse<SessionModel>>();

        return Response.Ok(new GetManyResponse<SessionModel> { Values = result.Value });
    }
}
