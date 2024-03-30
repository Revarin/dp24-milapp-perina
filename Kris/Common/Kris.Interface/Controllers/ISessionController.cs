using Kris.Interface.Models;
using Kris.Interface.Requests;
using Kris.Interface.Responses;

namespace Kris.Interface.Controllers;

public interface ISessionController
{
    Task<IdentityResponse?> CreateSession(CreateSessionRequest request, CancellationToken ct);
    Task<Response?> EditSession(EditSessionRequest request, CancellationToken ct);
    Task<IdentityResponse?> EditSessionUser(EditSessionUserRequest request, CancellationToken ct);
    Task<Response?> EndSession(PasswordRequest request, CancellationToken ct);
    Task<IdentityResponse?> JoinSession(JoinSessionRequest request, CancellationToken ct);
    Task<IdentityResponse?> LeaveSession(Guid sessionId, CancellationToken ct);
    Task<Response?> KickFromSession(Guid userId, CancellationToken ct);
    Task<GetOneResponse<SessionDetailModel>?> GetSession(Guid sessionId, CancellationToken ct);
    Task<GetManyResponse<SessionListModel>?> GetSessions(CancellationToken ct);
}
