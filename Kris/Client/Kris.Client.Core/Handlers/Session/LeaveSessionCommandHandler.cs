using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Responses;
using MediatR;
using System.Net;

namespace Kris.Client.Core.Handlers.Session;

public sealed class LeaveSessionCommandHandler : SessionHandler, IRequestHandler<LeaveSessionCommand, Result>
{
    public LeaveSessionCommandHandler(ISessionController sessionClient, IIdentityStore identityStore, IUserMapper userMapper)
        : base(sessionClient, identityStore, userMapper)
    {
    }

    public async Task<Result> Handle(LeaveSessionCommand request, CancellationToken cancellationToken)
    {
        using var t = Common.Metrics.SentryMetrics.TimerStart("RequestHandler");
        IdentityResponse response;

        try
        {
            response = await _sessionClient.LeaveSession(request.SessionId, cancellationToken);
        }
        catch (WebException)
        {
            return Result.Fail(new ConnectionError());
        }

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else if (response.IsBadRequest()) return Result.Fail(new BadOperationError(response.Message));
            else return Result.Fail(new ServerError(response.Message));
        }

        _identityStore.StoreIdentity(_userMapper.Map(response));

        return Result.Ok();
    }
}
