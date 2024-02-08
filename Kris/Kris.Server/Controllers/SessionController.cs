using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
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

    [HttpPost("Create")]
    [Authorize]
    public async Task<ActionResult<JwtTokenResponse>> CreateSession(CreateSessionRequest request, CancellationToken ct)
    {
        var user = await GetUserAsync(ct);
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

    public Task<ActionResult> EndSession(object request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult> GetAvailableSessions(object request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult> GetSession(object request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult> JoinSession(object request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
