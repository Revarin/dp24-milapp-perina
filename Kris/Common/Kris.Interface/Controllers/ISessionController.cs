using Kris.Interface.Models;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Kris.Interface.Controllers;

public interface ISessionController
{
    Task<ActionResult<JwtTokenResponse>> CreateSession(CreateSessionRequest request, CancellationToken ct);
    Task<ActionResult<JwtTokenResponse>> EditSession(EditSessionRequest request, CancellationToken ct);
    Task<ActionResult<JwtTokenResponse>> EndSession(CancellationToken ct);
    Task<ActionResult<JwtTokenResponse>> JoinSession(JoinSessionRequest request, CancellationToken ct);
    Task<ActionResult<JwtTokenResponse>> LeaveSession(Guid sessionId, CancellationToken ct);
    Task<ActionResult> KickFromSession(Guid userId, CancellationToken ct);
    Task<ActionResult<SessionModel>> GetSession(Guid sessionId, CancellationToken ct);
    Task<ActionResult<IEnumerable<SessionModel>>> GetSessions(CancellationToken ct);
}
