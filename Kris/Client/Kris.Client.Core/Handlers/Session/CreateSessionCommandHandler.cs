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

public sealed class CreateSessionCommandHandler : SessionHandler, IRequestHandler<CreateSessionCommand, Result>
{
    public CreateSessionCommandHandler(ISessionController sessionClient, IIdentityStore identityStore, IUserMapper userMapper)
        : base(sessionClient, identityStore, userMapper)
    {
    }

    public async Task<Result> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var httpRequest = new CreateSessionRequest
        {
            Name = request.Name,
            Password = request.Password
        };
        IdentityResponse response;

        try
        {
            response = await _sessionClient.CreateSession(httpRequest, cancellationToken);
        }
        catch (WebException)
        {
            return Result.Fail(new ConnectionError());
        }

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else if (response.IsBadRequest()) return Result.Fail(new EntityExistsError());
            else return Result.Fail(new ServerError(response.Message));
        }

        _identityStore.StoreIdentity(_userMapper.Map(response));

        return Result.Ok();
    }
}
