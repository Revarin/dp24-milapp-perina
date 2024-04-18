using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using MediatR;
using System.Net;

namespace Kris.Client.Core.Handlers.Session;

public sealed class EditSessionUserRoleCommandHandler : SessionHandler, IRequestHandler<EditSessionUserRoleCommand, Result>
{
    public EditSessionUserRoleCommandHandler(ISessionController sessionClient, IIdentityStore identityStore, IUserMapper userMapper)
        : base(sessionClient, identityStore, userMapper)
    {
    }

    public async Task<Result> Handle(EditSessionUserRoleCommand request, CancellationToken cancellationToken)
    {
        using var t = Common.Metrics.SentryMetrics.TimerStart("RequestHandler");
        var httpRequest = new EditSessionUserRoleRequest
        {
            UserId = request.UserId,
            UserType = request.NewRole
        };
        Response response;

        try
        {
            response = await _sessionClient.EditSessionUserRole(httpRequest, cancellationToken);
        }
        catch (WebException)
        {
            return Result.Fail(new ConnectionError());
        }

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError(response.Message));
            else if (response.IsNotFound()) return Result.Fail(new EntityNotFoundError(response.Message));
            else if (response.IsForbidden()) return Result.Fail(new ForbiddenError(response.Message));
            else return Result.Fail(new ServerError(response.Message));
        }

        return Result.Ok();
    }
}
