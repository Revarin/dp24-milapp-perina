using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using MediatR;

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
        var response = await _sessionClient.CreateSession(httpRequest, cancellationToken);

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
