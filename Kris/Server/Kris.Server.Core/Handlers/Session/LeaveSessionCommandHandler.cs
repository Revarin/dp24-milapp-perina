﻿using FluentResults;
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
    private readonly IUserMapper _userMapper;
    private readonly IJwtService _jwtService;

    public LeaveSessionCommandHandler(IUserMapper userMapper, IJwtService jwtService,
        ISessionRepository sessionRepository, ISessionMapper sessionMapper, IAuthorizationService authorizationService)
        : base(sessionRepository, sessionMapper, authorizationService)
    {
        _userMapper = userMapper;
        _jwtService = jwtService;
    }

    public async Task<Result<LoginResponse>> Handle(LeaveSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetWithUsersAsync(request.SessionId, cancellationToken);
        if (session == null) return Result.Fail(new EntityNotFoundError("Session", request.SessionId));

        var sessionUser = session.Users.Find(sessionUser => sessionUser.UserId == request.User.Id);
        if (sessionUser == null) return Result.Fail(new UserNotInSessionError());
        if (sessionUser.UserType == UserType.SuperAdmin) return Result.Fail(new InvalidOperationError("Session owner cannot leave session"));
        if (sessionUser.User == null) throw new NullableException();

        var user = sessionUser.User;

        user.CurrentSession = user.CurrentSessionId == session.Id ? null : user.CurrentSession;
        user.CurrentSessionId = user.CurrentSessionId == session.Id ? null : user.CurrentSessionId;
        session.Users.Remove(sessionUser);
        var updated = await _sessionRepository.UpdateAsync(session, cancellationToken);
        if (!updated) throw new DatabaseException("Failed to remove user from session");

        JwtToken jwt;
        if (user.CurrentSession?.Session != null)
        {
            jwt = _jwtService.CreateToken(user, user.CurrentSession.Session, user.CurrentSession.UserType);
        }
        else
        {
            jwt = _jwtService.CreateToken(user);
        }
        if (string.IsNullOrEmpty(jwt.Token)) throw new JwtException("Failed to create token");

        return Result.Ok(_userMapper.MapToLoginResponse(user, jwt));
    }
}
