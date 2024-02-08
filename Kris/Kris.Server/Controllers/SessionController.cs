using Kris.Interface.Controllers;
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
    public Task<ActionResult> CreateSession(object request, CancellationToken ct)
    {
        throw new NotImplementedException();
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
