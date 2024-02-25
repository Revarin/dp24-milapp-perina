using FluentResults;
using Kris.Common.Enums;
using Kris.Common.Models;
using Kris.Interface.Responses;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.Session;

public sealed class LeaveSessionCommandHandler : SessionHandler, IRequestHandler<LeaveSessionCommand, Result<LoginResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserMapper _userMapper;
    private readonly IJwtService _jwtService;

    public LeaveSessionCommandHandler(IUserRepository userRepository, IUserMapper userMapper, IJwtService jwtService,
        ISessionRepository sessionRepository, ISessionMapper sessionMapper, IAuthorizationService authorizationService)
        : base(sessionRepository, sessionMapper, authorizationService)
    {
        _userRepository = userRepository;
        _userMapper = userMapper;
        _jwtService = jwtService;
    }

    public async Task<Result<LoginResponse>> Handle(LeaveSessionCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetWithSessionsAsync(request.User.UserId, cancellationToken);
        if (user == null) throw new NullableException();

        var session = await _sessionRepository.GetAsync(request.SessionId, cancellationToken);
        if (session == null) return Result.Fail(new EntityNotFoundError("Session", request.SessionId));

        var leavingSessionUser = user.AllSessions.Find(sessionUser => sessionUser.SessionId == session.Id);
        if (leavingSessionUser == null) return Result.Fail(new UserNotInSessionError());
        if (leavingSessionUser.UserType == UserType.SuperAdmin) return Result.Fail(new InvalidOperationError("Session owner cannot leave session"));

        if (user.CurrentSession?.SessionId == leavingSessionUser.SessionId)
        {
            user.CurrentSession = null;
        }
        user.AllSessions.Remove(leavingSessionUser);
        await _userRepository.UpdateAsync(cancellationToken);

        var jwt = _jwtService.CreateToken(_userMapper.Map(user));
        if (string.IsNullOrEmpty(jwt.Token)) throw new JwtException("Failed to create token");

        return Result.Ok(_userMapper.MapToLoginResponse(user, jwt));
    }
}
