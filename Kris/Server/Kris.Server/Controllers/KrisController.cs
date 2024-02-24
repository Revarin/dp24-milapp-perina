using Kris.Common.Enums;
using Kris.Server.Common;
using Kris.Server.Core.Models;
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

    protected CurrentUserModel? CurrentUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = User.Identity?.Name;
        var sessionId = User.FindFirst(KrisClaimTypes.SessionId)?.Value;
        var sessionName = User.FindFirst(KrisClaimTypes.SessionName)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

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
}
