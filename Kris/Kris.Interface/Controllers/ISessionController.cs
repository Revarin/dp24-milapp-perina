using Kris.Interface.Requests;
using Kris.Interface.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Kris.Interface.Controllers;

public interface ISessionController
{
    Task<ActionResult<JwtTokenResponse>> CreateSession(CreateSessionRequest request, CancellationToken ct);
    Task<ActionResult> EndSession(object request, CancellationToken ct);
    Task<ActionResult> JoinSession(object request, CancellationToken ct);
    Task<ActionResult> GetSession(object request, CancellationToken ct);
    Task<ActionResult> GetAvailableSessions(object request, CancellationToken ct);
}
