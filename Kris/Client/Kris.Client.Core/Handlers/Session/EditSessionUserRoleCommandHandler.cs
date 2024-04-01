using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using MediatR;

namespace Kris.Client.Core.Handlers.Session;

public sealed class EditSessionUserRoleCommandHandler : SessionHandler, IRequestHandler<EditSessionUserRoleCommand, Result>
{
    public EditSessionUserRoleCommandHandler(ISessionController sessionClient, IIdentityStore identityStore, IUserMapper userMapper)
        : base(sessionClient, identityStore, userMapper)
    {
    }

    public async Task<Result> Handle(EditSessionUserRoleCommand request, CancellationToken cancellationToken)
    {
        var httpRequest = new EditSessionUserRoleRequest
        {
            UserId = request.UserId,
            UserType = request.NewRole
        };
        var response = await _sessionClient.EditSessionUserRole(httpRequest, cancellationToken);

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else if (response.IsNotFound()) return Result.Fail(new EntityNotFoundError());
            else if (response.IsForbidden()) return Result.Fail(new ForbiddenError());
            else return Result.Fail(new ServerError(response.Message));
        }

        return Result.Ok();
    }
}
