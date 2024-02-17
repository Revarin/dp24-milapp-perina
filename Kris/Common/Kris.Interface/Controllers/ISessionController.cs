using Kris.Interface.Models;
using Kris.Interface.Requests;
using Kris.Interface.Responses;

namespace Kris.Interface.Controllers;

public interface ISessionController
{
    Task<Response<JwtTokenResponse>> CreateSession(CreateSessionRequest request, CancellationToken ct);
    Task<Response<JwtTokenResponse>> EditSession(EditSessionRequest request, CancellationToken ct);
    Task<Response<JwtTokenResponse>> EndSession(CancellationToken ct);
    Task<Response<JwtTokenResponse>> JoinSession(JoinSessionRequest request, CancellationToken ct);
    Task<Response<JwtTokenResponse>> LeaveSession(Guid sessionId, CancellationToken ct);
    Task<Response<EmptyResponse>> KickFromSession(Guid userId, CancellationToken ct);
    Task<Response<GetOneResponse<SessionModel>>> GetSession(Guid sessionId, CancellationToken ct);
    Task<Response<GetManyResponse<SessionModel>>> GetSessions(CancellationToken ct);
}
