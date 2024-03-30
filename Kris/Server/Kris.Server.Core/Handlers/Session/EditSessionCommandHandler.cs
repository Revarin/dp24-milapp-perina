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

public sealed class EditSessionCommandHandler : SessionHandler, IRequestHandler<EditSessionCommand, Result>
{
    private readonly IPasswordService _passwordService;

    public EditSessionCommandHandler(IPasswordService passwordService,
        ISessionRepository sessionRepository, ISessionMapper sessionMapper, IAuthorizationService authorizationService)
        : base(sessionRepository, sessionMapper, authorizationService)
    {
        _passwordService = passwordService;
    }

    public async Task<Result> Handle(EditSessionCommand request, CancellationToken cancellationToken)
    {
        var user = request.User;
        if (!user.SessionId.HasValue || !user.UserType.HasValue)
            return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var authResult = await _authorizationService.AuthorizeAsync(user, UserType.Admin, cancellationToken);
        if (!authResult.IsAuthorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var session = await _sessionRepository.GetWithAllAsync(user.SessionId.Value, cancellationToken);
        if (session == null) throw new NullableException();
        if (session.Name != user.SessionName) return Result.Fail(new UnauthorizedError("Invalid token"));

        var passwordVerified = _passwordService.VerifyPassword(session.Password, request.EditSession.Password);
        if (!passwordVerified) return Result.Fail(new InvalidCredentialsError());

        session.Name = request.EditSession.NewName;
        if (!string.IsNullOrEmpty(request.EditSession.NewPassword))
        {
            session.Password = _passwordService.HashPassword(request.EditSession.NewPassword);
        }
        await _sessionRepository.UpdateAsync(cancellationToken);

        return Result.Ok();
    }
}
