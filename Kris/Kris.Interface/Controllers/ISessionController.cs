using Microsoft.AspNetCore.Mvc;

namespace Kris.Interface.Controllers;

public interface ISessionController
{
    Task<ActionResult> CreateSession(object request, CancellationToken ct);
    Task<ActionResult> EndSession(object request, CancellationToken ct);
    Task<ActionResult> JoinSession(object request, CancellationToken ct);
    Task<ActionResult> GetSession(object request, CancellationToken ct);
    Task<ActionResult> GetAvailableSessions(object request, CancellationToken ct);
}
