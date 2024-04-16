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

public sealed class EndSessionCommandHandler : SessionHandler, IRequestHandler<EndSessionCommand, Result>
{
    public EndSessionCommandHandler(ISessionController sessionClient, IIdentityStore identityStore, IUserMapper userMapper)
        : base(sessionClient, identityStore, userMapper)
    {
    }

    public async Task<Result> Handle(EndSessionCommand request, CancellationToken cancellationToken)
    {
        var httpRequest = new PasswordRequest { Password = request.Password };
        Response response;

        try
        {
            response = await _sessionClient.EndSession(httpRequest, cancellationToken);
        }
        catch (WebException)
        {
            return Result.Fail(new ConnectionError());
        }

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else if (response.IsForbidden()) return Result.Fail(new ForbiddenError());
            else if (response.IsNotFound()) return Result.Fail(new EntityNotFoundError());
            else return Result.Fail(new ServerError(response.Message));
        }

        _identityStore.ClearCurrentSession();

        return Result.Ok();
    }
}
