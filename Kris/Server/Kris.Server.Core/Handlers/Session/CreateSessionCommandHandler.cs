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

public sealed class CreateSessionCommandHandler : SessionHandler, IRequestHandler<CreateSessionCommand, Result<IdentityResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserMapper _userMapper;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public CreateSessionCommandHandler(IUserRepository userRepository, IUserMapper userMapper, IPasswordService passwordService, IJwtService jwtService,
        ISessionRepository sessionRepository, ISessionMapper sessionMapper, IAuthorizationService authorizationService)
        : base(sessionRepository, sessionMapper, authorizationService)
    {
        _userRepository = userRepository;
        _userMapper = userMapper;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task<Result<IdentityResponse>> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetWithSessionsAsync(request.User.UserId, cancellationToken);
        if (user == null) throw new DatabaseException("User not found");

        var sessionExists = await _sessionRepository.ExistsAsync(request.CreateSession.Name, cancellationToken);
        if (sessionExists) return Result.Fail(new EntityExistsError("Session", request.CreateSession.Name));

        var session = new SessionEntity
        {
            Name = request.CreateSession.Name,
            Password = _passwordService.HashPassword(request.CreateSession.Password),
            Created = DateTime.UtcNow
        };
        var sessionUser = new SessionUserEntity
        {
            SessionId = session.Id,
            UserId = user.Id,
            UserType = UserType.SuperAdmin,
            Joined = DateTime.UtcNow
        };
        var globalConversation = new ConversationEntity
        {
            ConversationType = ConversationType.Global,
            SessionId = session.Id,
        };
        globalConversation.Users.Add(sessionUser);
        session.Conversations.Add(globalConversation);
        session.Users.Add(sessionUser);
        user.CurrentSession = sessionUser;
        var sessionEntity = await _sessionRepository.InsertAsync(session, cancellationToken);
        if (sessionEntity == null) throw new DatabaseException("Failed to insert Session");

        var jwt = _jwtService.CreateToken(_userMapper.Map(user));
        if (string.IsNullOrEmpty(jwt.Token)) throw new JwtException("Failed to create token");

        return Result.Ok(_userMapper.MapToLoginResponse(user, jwt));
    }
}
