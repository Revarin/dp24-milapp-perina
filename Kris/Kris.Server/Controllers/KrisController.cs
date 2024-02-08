using Kris.Server.Core.Models;
using Kris.Server.Core.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kris.Server.Controllers;

public abstract class KrisController : ControllerBase
{
    protected readonly IMediator _mediator;

    protected KrisController(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected async Task<UserModel?> GetUserAsync(CancellationToken ct)
    {
        if (User?.Identity?.Name == null) return null;

        var query = new GetUserQuery { Id = Guid.Parse(User.Identity.Name) };
        var result = await _mediator.Send(query, ct);

        if (result.IsFailed) return null;
        return result.Value;
    }
}
