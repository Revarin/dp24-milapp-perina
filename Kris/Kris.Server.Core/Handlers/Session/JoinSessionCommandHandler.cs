using FluentResults;
using Kris.Common.Enums;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Common.Models;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Models;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.Session;

public sealed class JoinSessionCommandHandler : SessionHandler, IRequestHandler<JoinSessionCommand, Result<JwtToken>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public JoinSessionCommandHandler(IUserRepository userRepository, IPasswordService passwordService, IJwtService jwtService,
        ISessionRepository sessionRepository, ISessionMapper sessionMapper, IAuthorizationService authorizationService)
        : base(sessionRepository, sessionMapper, authorizationService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task<Result<JwtToken>> Handle(JoinSessionCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.User.Id, cancellationToken);
        if (user == null) throw new DatabaseException("User not found");

        var session = await _sessionRepository.GetAsync(request.JoinSession.Id, cancellationToken);
        if (session == null) return Result.Fail(new EntityNotFoundError("Session", request.JoinSession.Id));
        if (session.Password == null) throw new DatabaseException("Password missing in database");

        var passwordVerified = _passwordService.VerifyPassword(session.Password, request.JoinSession.Password);
        if (!passwordVerified) return Result.Fail(new InvalidCredentialsError());

        user.Session = new SessionUserEntity
        {
            UserId = user.Id,
            SessionId = session.Id,
            Joined = DateTime.UtcNow,
            UserType = UserType.Basic
        };
        var updated = await _userRepository.UpdateAsync(user, cancellationToken);
        if (!updated) throw new DatabaseException("Failed to assign session to user");

        var jwt = _jwtService.CreateToken(user, session, UserType.Basic);
        if (string.IsNullOrEmpty(jwt.Token)) throw new JwtException("Failed to create token");

        return Result.Ok(jwt);
    }
}
