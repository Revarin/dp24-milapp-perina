using Kris.Common.Enums;
using Kris.Interface.Responses;
using Kris.Server.Common;
using Kris.Server.Core.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Kris.Server.Controllers;

public abstract class KrisHub<T> : Hub<T> where T : class
{
    protected readonly IMediator _mediator;

    protected KrisHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected CurrentUserModel? CurrentUser()
    {
        if (Context.User == null) return null;

        var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = Context.User.Identity?.Name;
        var sessionId = Context.User.FindFirst(KrisClaimTypes.SessionId)?.Value;
        var sessionName = Context.User.FindFirst(KrisClaimTypes.SessionName)?.Value;
        var role = Context.User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName)) return null;

        return new CurrentUserModel
        {
            UserId = Guid.Parse(userId),
            Login = userName,
            SessionId = string.IsNullOrEmpty(sessionId) ? null : Guid.Parse(sessionId),
            UserType = string.IsNullOrEmpty(role) ? null : Enum.Parse<UserType>(role),
            SessionName = sessionName
        };
    }

    protected Response Ok(string? message = null) => new Response { Status = StatusCodes.Status200OK, Message = message };
    protected Response Unauthorized(string? message = null) => new Response { Status = StatusCodes.Status401Unauthorized, Message = message };
    protected Response NotFound(string? message = null) => new Response { Status = StatusCodes.Status404NotFound, Message = message };
    protected Response InternalError(string? message = null) => new Response { Status = StatusCodes.Status500InternalServerError, Message = message };
}
