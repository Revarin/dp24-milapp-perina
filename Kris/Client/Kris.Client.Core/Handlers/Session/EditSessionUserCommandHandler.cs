using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using MediatR;

namespace Kris.Client.Core.Handlers.Session;

public sealed class EditSessionUserCommandHandler : SessionHandler, IRequestHandler<EditSessionUserCommand, Result>
{
    public EditSessionUserCommandHandler(ISessionController sessionClient, IIdentityStore identityStore, IUserMapper userMapper)
        : base(sessionClient, identityStore, userMapper)
    {
    }

    public async Task<Result> Handle(EditSessionUserCommand request, CancellationToken cancellationToken)
    {
        var httpRequest = new EditSessionUserRequest
        {
            Nickname = request.UserName,
            Symbol = request.UserSymbol
        };
        var response = await _sessionClient.EditSessionUser(httpRequest, cancellationToken);

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else return Result.Fail(new ServerError(response.Message));
        }

        _identityStore.StoreIdentity(_userMapper.Map(response));

        return Result.Ok();
    }
}
