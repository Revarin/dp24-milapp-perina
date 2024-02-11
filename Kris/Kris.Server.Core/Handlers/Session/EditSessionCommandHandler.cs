using FluentResults;
using Kris.Common.Enums;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Common.Models;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.Session;

public sealed class EditSessionCommandHandler : SessionHandler, IRequestHandler<EditSessionCommand, Result<JwtToken>>
{
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;
    private readonly IUserMapper _userMapper;

    public EditSessionCommandHandler(IPasswordService passwordService, IJwtService jwtService, IUserMapper userMapper,
        ISessionRepository sessionRepository, ISessionMapper sessionMapper, IAuthorizationService authorizationService)
        : base(sessionRepository, sessionMapper, authorizationService)
    {
        _passwordService = passwordService;
        _jwtService = jwtService;
        _userMapper = userMapper;
    }

    public async Task<Result<JwtToken>> Handle(EditSessionCommand request, CancellationToken cancellationToken)
    {
        var user = request.User;
        if (!user.SessionId.HasValue) throw new JwtException("Token missing session");
        if (!user.UserType.HasValue) throw new JwtException("Token missing type");

        var authorized = await _authorizationService.AuthorizeAsync(user, UserType.Admin, cancellationToken);
        if (!authorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType.ToString()));

        var session = await _sessionRepository.GetWithUsersAsync(user.SessionId.Value, cancellationToken);
        if (session == null) throw new DatabaseException("Session not found");
        if (session.Name != user.SessionName) return Result.Fail(new UnauthorizedError("Invalid token"));

        session.Name = request.EditSession.Name;
        session.Password = _passwordService.HashPassword(request.EditSession.Password);
        var updated = await _sessionRepository.UpdateAsync(session, cancellationToken);
        if (!updated) throw new DatabaseException("Failed to update session");

        var jwt = _jwtService.CreateToken(_userMapper.Map(user), session, user.UserType.Value);
        if (string.IsNullOrEmpty(jwt.Token)) throw new JwtException("Failed to create token");

        return Result.Ok(jwt);
    }
}
