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

public sealed class KickFromSessionCommandHandler : SessionHandler, IRequestHandler<KickFromSessionCommand, Result>
{
    public KickFromSessionCommandHandler(ISessionRepository sessionRepository, ISessionMapper sessionMapper, IAuthorizationService authorizationService)
        : base(sessionRepository, sessionMapper, authorizationService)
    {
    }

    public async Task<Result> Handle(KickFromSessionCommand request, CancellationToken cancellationToken)
    {
        var user = request.User;
        if (!user.SessionId.HasValue) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var authorized = await _authorizationService.AuthorizeAsync(user, UserType.Admin, cancellationToken);
        if (!authorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var session = await _sessionRepository.GetWithUsersAsync(user.SessionId.Value, cancellationToken);
        if (session == null) throw new NullableException();

        var kickedUser = session.Users.Find(sessionUser => sessionUser.UserId == request.UserId);
        if (kickedUser == null) return Result.Fail(new UserNotInSessionError());
        if (kickedUser.UserType == UserType.SuperAdmin) return Result.Fail(new InvalidOperationError("Cannot kick session owner"));
        if (kickedUser.User == null) throw new NullableException();

        kickedUser.User.CurrentSession = kickedUser.User.CurrentSession?.SessionId == session.Id ? null : kickedUser.User.CurrentSession;
        session.Users.Remove(kickedUser);
        if (kickedUser.User.CurrentSession?.SessionId == session.Id)
        {
            kickedUser.User.CurrentSession = null;
        }
        session.Users.Remove(kickedUser);
        var updated = await _sessionRepository.UpdateAsync(session, cancellationToken);
        if (!updated) throw new DatabaseException("Failed to remove user from session");

        return Result.Ok();
    }
}
