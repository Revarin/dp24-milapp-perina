using FluentResults;
using Kris.Common.Enums;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.Session;

public sealed class EditSessionUserRoleCommandHandler : SessionHandler, IRequestHandler<EditSessionUserRoleCommand, Result>
{
    public EditSessionUserRoleCommandHandler(ISessionRepository sessionRepository, ISessionMapper sessionMapper, IAuthorizationService authorizationService)
        : base(sessionRepository, sessionMapper, authorizationService)
    {
    }

    public async Task<Result> Handle(EditSessionUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = request.User;
        if (!user.SessionId.HasValue || !user.UserType.HasValue)
            return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var authResult = await _authorizationService.AuthorizeAsync(user, UserType.Admin, cancellationToken);
        if (!authResult.IsAuthorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var session = await _sessionRepository.GetWithAllAsync(user.SessionId.Value, cancellationToken);
        if (session == null) throw new NullableException();

        var sessionUser = session.Users.FirstOrDefault(su => su.UserId == request.EditSessionUserRole.UserId);
        if (sessionUser == null) return Result.Fail(new EntityNotFoundError("User", request.EditSessionUserRole.UserId));
        if (sessionUser.UserType == UserType.SuperAdmin) return Result.Fail(new InvalidOperationError("Cannot change role of session owner"));

        sessionUser.UserType = request.EditSessionUserRole.UserType;
        await _sessionRepository.UpdateAsync(cancellationToken);

        return Result.Ok();
    }
}
