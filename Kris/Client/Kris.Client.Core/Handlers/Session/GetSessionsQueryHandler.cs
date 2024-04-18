using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Responses;
using MediatR;
using System.Net;

using ClientSessionListModel = Kris.Client.Core.Models.SessionListModel;
using InterfaceSessionListModel = Kris.Interface.Models.SessionListModel;

namespace Kris.Client.Core.Handlers.Session;

public sealed class GetSessionsQueryHandler : SessionHandler, IRequestHandler<GetSessionsQuery, Result<AvailableSessionsModel>>
{
    private readonly ISessionMapper _sessionMapper;

    public GetSessionsQueryHandler(ISessionMapper sessionMapper,
        ISessionController sessionClient, IIdentityStore identityStore, IUserMapper userMapper)
        : base(sessionClient, identityStore, userMapper)
    {
        _sessionMapper = sessionMapper;
    }

    public async Task<Result<AvailableSessionsModel>> Handle(GetSessionsQuery request, CancellationToken cancellationToken)
    {
        using var t = Common.Metrics.SentryMetrics.TimerStart("RequestHandler");
        GetManyResponse<InterfaceSessionListModel> response;

        try
        {
            response = await _sessionClient.GetSessions(cancellationToken);
        }
        catch (WebException)
        {
            return Result.Fail(new ConnectionError());
        }

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else return Result.Fail(new ServerError(response.Message));
        }

        var allSession = response.Values.Select(_sessionMapper.Map);
        var user = _identityStore.GetIdentity();
        var sessions = _identityStore.GetJoinedSessions();

        var currentSession = allSession.FirstOrDefault(s => user.CurrentSession?.Id == s.Id);
        var joinedSessions = allSession.IntersectBy(sessions, s => s.Id).Where(s => user.CurrentSession?.Id != s.Id).ToList();
        var otherSessions = allSession.ExceptBy(sessions, s => s.Id).Where(s => user.CurrentSession?.Id != s.Id).ToList();

        var result = Result.Ok(new AvailableSessionsModel
        {
            UserType = user.CurrentSession?.UserType,
            CurrentSession = currentSession,
            JoinedSessions = joinedSessions,
            OtherSessions = otherSessions
        });
        return result;
    }
}
