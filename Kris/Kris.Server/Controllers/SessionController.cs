using Kris.Common.Enums;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using Kris.Server.Attributes;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Requests;
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
    public async Task<ActionResult<JwtTokenResponse>> CreateSession(CreateSessionRequest request, CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Unauthorized();

        var command = new CreateSessionCommand { CreateSession = request, User = user };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<EntityExistsError>()) return BadRequest(result.Errors.Select(e => e.Message));
            else return BadRequest();
        }

        return Ok(new JwtTokenResponse { Token = result.Value.Token});
    }

    [HttpDelete]
    [AuthorizeRoles(UserType.Admin, UserType.SuperAdmin)]
    public async Task<ActionResult<JwtTokenResponse>> EndSession(CancellationToken ct)
    {
        var user = CurrentUser();
        if (user == null) return Unauthorized();

        var command = new EndSessionCommand() { User = user };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Unauthorized(result.Errors.Select(e => e.Message));
            else if (result.HasError<EntityNotFoundError>()) return NotFound(result.Errors.Select(e => e.Message));
            else return BadRequest();
        }

        return Ok(new JwtTokenResponse { Token = result.Value.Token });
    }

    [HttpGet]
    public Task<ActionResult> GetAvailableSessions(object request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{sessionId:guid}")]
    public Task<ActionResult> GetSession(object request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    [HttpPost("Join")]
    public Task<ActionResult> JoinSession(object request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
