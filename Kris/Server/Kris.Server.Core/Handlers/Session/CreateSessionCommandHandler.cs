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

public sealed class CreateSessionCommandHandler : SessionHandler, IRequestHandler<CreateSessionCommand, Result<LoginResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public CreateSessionCommandHandler(IUserRepository userRepository, IPasswordService passwordService, IJwtService jwtService,
        ISessionRepository sessionRepository, ISessionMapper sessionMapper, IAuthorizationService authorizationService)
        : base(sessionRepository, sessionMapper, authorizationService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task<Result<LoginResponse>> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetWithCurrentSessionAsync(request.User.Id, cancellationToken);
        if (user == null) throw new DatabaseException("User not found");

        var sessionExists = await _sessionRepository.ExistsAsync(request.CreateSession.Name, cancellationToken);
        if (sessionExists) return Result.Fail(new EntityExistsError("Session", request.CreateSession.Name));

        var session = new SessionEntity
        {
            Id = Guid.NewGuid(),
            Name = request.CreateSession.Name,
            Password = _passwordService.HashPassword(request.CreateSession.Password),
            Created = DateTime.UtcNow
        };
        session.Users.Add(new SessionUserEntity
        {
            SessionId = session.Id,
            UserId = user.Id,
            UserType = UserType.SuperAdmin,
            Joined = DateTime.UtcNow
        });
        var sessionEntity = await _sessionRepository.InsertAsync(session, cancellationToken);
        if (sessionEntity == null) throw new DatabaseException("Failed to insert Session");

        var jwt = _jwtService.CreateToken(user, sessionEntity, UserType.SuperAdmin);
        if (string.IsNullOrEmpty(jwt.Token)) throw new JwtException("Failed to create token");

        return Result.Ok(new LoginResponse
        {
            UserId = user.Id,
            Login = user.Login,
            SessionId = user.CurrentSession?.SessionId,
            SessionName = user.CurrentSession?.Session?.Name,
            UserType = user.CurrentSession?.UserType,
            Token = jwt.Token
        });
    }
}
