using Kris.Interface.Models;
using Kris.Interface.Requests;
using Kris.Interface.Responses;

namespace Kris.Interface.Controllers;

public interface ISessionController
{
    Task<Response<LoginResponse>> CreateSession(CreateSessionRequest request, CancellationToken ct);
    Task<Response<LoginResponse>> EditSession(EditSessionRequest request, CancellationToken ct);
    Task<Response<LoginResponse>> EndSession(CancellationToken ct);
    Task<Response<LoginResponse>> JoinSession(JoinSessionRequest request, CancellationToken ct);
    Task<Response<LoginResponse>> LeaveSession(Guid sessionId, CancellationToken ct);
    Task<Response<EmptyResponse>> KickFromSession(Guid userId, CancellationToken ct);
    Task<Response<GetOneResponse<SessionModel>>> GetSession(Guid sessionId, CancellationToken ct);
    Task<Response<GetManyResponse<SessionModel>>> GetSessions(CancellationToken ct);
}
