using FluentResults;
using Kris.Common.Enums;
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
    private readonly IConversationRepository _conversationRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public EndSessionCommandHandler(IConversationRepository conversationRepository, IPasswordService passwordService, IJwtService jwtService,
        ISessionRepository sessionRepository, ISessionMapper sessionMapper, IAuthorizationService authorizationService)
        : base(sessionRepository, sessionMapper, authorizationService)
    {
        _conversationRepository = conversationRepository;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task<Result> Handle(EndSessionCommand request, CancellationToken cancellationToken)
    {
        var user = request.User;
        if (!user.SessionId.HasValue) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var authResult = await _authorizationService.AuthorizeAsync(user, UserType.SuperAdmin, cancellationToken);
        if (!authResult.IsAuthorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var session = await _sessionRepository.GetWithAllAsync(user.SessionId.Value, cancellationToken);
        if (session == null) throw new DatabaseException("Session not found");

        var passwordVerified = _passwordService.VerifyPassword(session.Password, request.Password);
        if (!passwordVerified) return Result.Fail(new InvalidCredentialsError());

        // Null current session for users currently in this session
        foreach (var u in session.Users)
        {
            if (u.User?.CurrentSession?.SessionId == session.Id)
            {
                u.User.CurrentSession = null;
            }
        }
        // Cascade delete conversations
        foreach (var c in session.Conversations)
        {
            await _conversationRepository.DeleteAsync(c, cancellationToken);
        }

        await _sessionRepository.DeleteAsync(session, cancellationToken);

        user.SessionId = null;
        user.SessionName = null;
        user.UserType = null;
        var jwt = _jwtService.CreateToken(user);
        if (string.IsNullOrEmpty(jwt.Token)) throw new JwtException("Failed to create token");

        return Result.Ok();
    }
}
