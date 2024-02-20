using FluentResults;
using Kris.Common.Enums;
using Kris.Interface.Responses;
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

    public async Task<Result> Handle(EditSessionCommand request, CancellationToken cancellationToken)
    {
        var currentUser = request.User;
        if (!currentUser.SessionId.HasValue || !currentUser.UserType.HasValue)
            return Result.Fail(new UnauthorizedError(currentUser.Login, currentUser.SessionName, currentUser.UserType));

        var authorized = await _authorizationService.AuthorizeAsync(currentUser, UserType.Admin, cancellationToken);
        if (!authorized) return Result.Fail(new UnauthorizedError(currentUser.Login, currentUser.SessionName, currentUser.UserType));

        var session = await _sessionRepository.GetWithUsersAsync(currentUser.SessionId.Value, cancellationToken);
        if (session == null) throw new NullableException();
        if (session.Name != currentUser.SessionName) return Result.Fail(new UnauthorizedError("Invalid token"));

        session.Name = request.EditSession.Name;
        session.Password = _passwordService.HashPassword(request.EditSession.Password);
        var updated = await _sessionRepository.UpdateAsync(session, cancellationToken);
        if (!updated) throw new DatabaseException("Failed to update session");

        var jwt = _jwtService.CreateToken(_userMapper.Map(currentUser), session, currentUser.UserType.Value);
        if (string.IsNullOrEmpty(jwt.Token)) throw new JwtException("Failed to create token");

        return Result.Ok();
    }
}
