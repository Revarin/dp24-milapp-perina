using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using MediatR;

namespace Kris.Client.Core.Handlers.Session;

public sealed class EditSessionCommandHandler : SessionHandler, IRequestHandler<EditSessionCommand, Result>
{
    public EditSessionCommandHandler(ISessionController sessionClient, IIdentityStore identityStore, IUserMapper userMapper)
        : base(sessionClient, identityStore, userMapper)
    {
    }

    public async Task<Result> Handle(EditSessionCommand request, CancellationToken cancellationToken)
    {
        var httpRequest = new EditSessionRequest
        {
            NewName = request.NewName,
            NewPassword = request.NewPassword,
            Password = request.Password
        };
        var result = await _sessionClient.EditSession(httpRequest, cancellationToken);

        if (!result.IsSuccess())
        {
            if (result.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else if (result.IsForbidden()) return Result.Fail(new ForbiddenError());
            else return Result.Fail(new ServerError(result.Message));
        }

        return Result.Ok();
        throw new NotImplementedException();
    }
}
