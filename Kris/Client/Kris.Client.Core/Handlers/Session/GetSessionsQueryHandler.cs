using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using MediatR;

namespace Kris.Client.Core.Handlers.Session;

public sealed class GetSessionsQueryHandler : SessionHandler, IRequestHandler<GetSessionsQuery, Result<AvailableSessionsModel>>
{
    private readonly IIdentityStore _identityStore;
    private readonly ISessionMapper _sessionMapper;

    public GetSessionsQueryHandler(IIdentityStore identityStore, ISessionMapper sessionMapper,
        ISessionController sessionClient, IUserMapper userMapper)
        : base(sessionClient, userMapper)
    {
        _identityStore = identityStore;
        _sessionMapper = sessionMapper;
    }

    public async Task<Result<AvailableSessionsModel>> Handle(GetSessionsQuery request, CancellationToken cancellationToken)
    {
        var response = await _sessionClient.GetSessions(cancellationToken);

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else return Result.Fail(new ServerError(response.Message));
        }

        var allSession = response.Values.Select(_sessionMapper.Map);
        var user = _identityStore.GetIdentity();
        var sessions = _identityStore.GetJoinedSessions();

        var currentSession = allSession.FirstOrDefault(session => session.Id == user.SessionId);
        var joinedSessions = allSession.IntersectBy(sessions, s => s.Id).Where(s => s.Id != user.SessionId);
        var otherSessions = allSession.Except(joinedSessions).Where(s => s.Id != user.SessionId);

        return Result.Ok(new AvailableSessionsModel
        {
            CurrentSession = currentSession,
            JoinedSessions = joinedSessions,
            OtherSessions = otherSessions
        });
    }
}
