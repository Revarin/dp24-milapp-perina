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

        var authResult = await _authorizationService.AuthorizeAsync(user, UserType.Admin, cancellationToken);
        if (!authResult.IsAuthorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var session = await _sessionRepository.GetWithAllAsync(user.SessionId.Value, cancellationToken);
        if (session == null) throw new NullableException();

        var kickedSessionUser = session.Users.Find(sessionUser => sessionUser.UserId == request.UserId);
        if (kickedSessionUser == null) return Result.Fail(new UserNotInSessionError());
        if (kickedSessionUser.UserType == UserType.SuperAdmin) return Result.Fail(new InvalidOperationError("Cannot kick session owner"));
        if (kickedSessionUser.User == null) throw new NullableException();

        if (kickedSessionUser.User.CurrentSession?.SessionId == session.Id)
        {
            kickedSessionUser.User.CurrentSession = null;
        }
        session.Users.Remove(kickedSessionUser);
        await _sessionRepository.UpdateAsync(cancellationToken);

        return Result.Ok();
    }
}
