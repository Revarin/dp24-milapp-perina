using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using MediatR;

namespace Kris.Client.Core.Handlers.Session;

public sealed class LeaveSessionCommandHandler : SessionHandler, IRequestHandler<LeaveSessionCommand, Result>
{
    public LeaveSessionCommandHandler(ISessionController sessionClient, IIdentityStore identityStore, IUserMapper userMapper)
        : base(sessionClient, identityStore, userMapper)
    {
    }

    // TODO: BAD IDENTITY STORE
    public async Task<Result> Handle(LeaveSessionCommand request, CancellationToken cancellationToken)
    {
        var response = await _sessionClient.LeaveSession(request.SessionId, cancellationToken);

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else if (response.IsBadRequest()) return Result.Fail(new EntityNotFoundError());
            else return Result.Fail(new ServerError(response.Message));
        }

        _identityStore.StoreIdentity(_userMapper.Map(response));

        return Result.Ok();
    }
}
