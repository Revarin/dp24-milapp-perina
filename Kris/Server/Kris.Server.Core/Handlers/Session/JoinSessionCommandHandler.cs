using FluentResults;
using Kris.Common.Enums;
using Kris.Interface.Responses;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Models;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.Session;

public sealed class JoinSessionCommandHandler : SessionHandler, IRequestHandler<JoinSessionCommand, Result<IdentityResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserMapper _userMapper;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public JoinSessionCommandHandler(IUserRepository userRepository, IUserMapper userMapper, IPasswordService passwordService, IJwtService jwtService,
        ISessionRepository sessionRepository, ISessionMapper sessionMapper, IAuthorizationService authorizationService)
        : base(sessionRepository, sessionMapper, authorizationService)
    {
        _userRepository = userRepository;
        _userMapper = userMapper;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task<Result<IdentityResponse>> Handle(JoinSessionCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetWithSessionsAsync(request.User.UserId, cancellationToken);
        if (user == null) throw new NullableException();

        var session = await _sessionRepository.GetAsync(request.JoinSession.Id, cancellationToken);
        if (session == null) return Result.Fail(new EntityNotFoundError("Session", request.JoinSession.Id));

        var passwordVerified = _passwordService.VerifyPassword(session.Password, request.JoinSession.Password);
        if (!passwordVerified) return Result.Fail(new InvalidCredentialsError());

        var sessionUser = user.AllSessions.Find(sessionUser => sessionUser.SessionId == session.Id);
        if (sessionUser == null)
        {
            sessionUser = new SessionUserEntity
            {
                UserId = user.Id,
                SessionId = session.Id,
                Joined = DateTime.Now,
                UserType = UserType.Basic,
                Nickname = user.Login
            };
            user.AllSessions.Add(sessionUser);
        }
        user.CurrentSession = sessionUser;
        await _userRepository.UpdateAsync(cancellationToken);

        var jwt = _jwtService.CreateToken(_userMapper.Map(user));
        if (string.IsNullOrEmpty(jwt.Token)) throw new JwtException("Failed to create token");

        return Result.Ok(_userMapper.MapToLoginResponse(user, jwt));
    }
}
