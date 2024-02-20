using Kris.Interface.Models;
using Kris.Interface.Requests;
using Kris.Interface.Responses;

namespace Kris.Interface.Controllers;

public interface ISessionController
{
    Task<LoginResponse?> CreateSession(CreateSessionRequest request, CancellationToken ct);
    Task<LoginResponse?> EditSession(EditSessionRequest request, CancellationToken ct);
    Task<LoginResponse?> EndSession(CancellationToken ct);
    Task<LoginResponse?> JoinSession(JoinSessionRequest request, CancellationToken ct);
    Task<LoginResponse?> LeaveSession(Guid sessionId, CancellationToken ct);
    Task<Response?> KickFromSession(Guid userId, CancellationToken ct);
    Task<GetOneResponse<SessionModel>?> GetSession(Guid sessionId, CancellationToken ct);
    Task<GetManyResponse<SessionModel>?> GetSessions(CancellationToken ct);
}
