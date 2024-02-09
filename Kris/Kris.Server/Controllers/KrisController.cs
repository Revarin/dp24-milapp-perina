using Kris.Server.Core.Models;
using Kris.Server.Core.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return null;

        var query = new GetUserQuery { Id = Guid.Parse(userId) };
        var result = await _mediator.Send(query, ct);

        if (result.IsFailed) return null;
        return result.Value;
    }
}
