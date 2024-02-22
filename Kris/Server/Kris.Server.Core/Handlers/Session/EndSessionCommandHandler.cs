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

public sealed class EndSessionCommandHandler : SessionHandler, IRequestHandler<EndSessionCommand, Result>
{
    private readonly IJwtService _jwtService;
    private readonly IUserMapper _userMapper;

    public EndSessionCommandHandler(IJwtService jwtService, IUserMapper userMapper,
        ISessionRepository sessionRepository, ISessionMapper sessionMapper, IAuthorizationService authorizationService)
        : base(sessionRepository, sessionMapper, authorizationService)
    {
        _jwtService = jwtService;
        _userMapper = userMapper;
    }

    public async Task<Result> Handle(EndSessionCommand request, CancellationToken cancellationToken)
    {
        var user = request.User;
        if (!user.SessionId.HasValue) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var authorized = await _authorizationService.AuthorizeAsync(user, UserType.SuperAdmin, cancellationToken);
        if (!authorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var session = await _sessionRepository.GetWithUsersAsync(user.SessionId.Value, cancellationToken);
        if (session == null) throw new DatabaseException("Session not found");

        foreach (var u in session.Users)
        {
            if (u.User?.CurrentSession?.SessionId == session.Id)
            {
                u.User.CurrentSession = null;
            }
        }

        var deleted = await _sessionRepository.DeleteAsync(user.SessionId.Value, cancellationToken);
        if (!deleted) return Result.Fail(new EntityNotFoundError("Session", user.SessionId.Value));

        var jwt = _jwtService.CreateToken(_userMapper.Map(user));
        if (string.IsNullOrEmpty(jwt.Token)) throw new JwtException("Failed to create token");

        return Result.Ok();
    }
}
