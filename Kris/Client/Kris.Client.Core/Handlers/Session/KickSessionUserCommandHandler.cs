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

public sealed class KickSessionUserCommandHandler : SessionHandler, IRequestHandler<KickSessionUserCommand, Result>
{
    public KickSessionUserCommandHandler(ISessionController sessionClient, IIdentityStore identityStore, IUserMapper userMapper)
        : base(sessionClient, identityStore, userMapper)
    {
    }

    public async Task<Result> Handle(KickSessionUserCommand request, CancellationToken cancellationToken)
    {
        using var t = Common.Metrics.SentryMetrics.TimerStart("RequestHandler");
        Response response;

        try
        {
            response = await _sessionClient.KickFromSession(request.UserId, cancellationToken);
        }
        catch (WebException)
        {
            return Result.Fail(new ConnectionError());
        }

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError(response.Message));
            else if (response.IsForbidden()) return Result.Fail(new ForbiddenError(response.Message));
            else if (response.IsNotFound()) return Result.Fail(new EntityNotFoundError(response.Message));
            else return Result.Fail(new ServerError(response.Message));
        }

        return Result.Ok();
    }
}
