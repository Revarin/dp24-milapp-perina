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
        var user = request.User;
        if (!user.SessionId.HasValue || !user.UserType.HasValue)
            return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var authResult = await _authorizationService.AuthorizeAsync(user, UserType.Admin, cancellationToken);
        if (!authResult.IsAuthorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var session = await _sessionRepository.GetWithUsersAsync(user.SessionId.Value, cancellationToken);
        if (session == null) throw new NullableException();
        if (session.Name != user.SessionName) return Result.Fail(new UnauthorizedError("Invalid token"));

        session.Name = request.EditSession.Name;
        session.Password = _passwordService.HashPassword(request.EditSession.Password);
        await _sessionRepository.UpdateAsync(cancellationToken);

        user.SessionName = session.Name;
        var jwt = _jwtService.CreateToken(user);
        if (string.IsNullOrEmpty(jwt.Token)) throw new JwtException("Failed to create token");

        return Result.Ok();
    }
}
