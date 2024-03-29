using FluentResults;
using Kris.Interface.Responses;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.Session;

public sealed class EditSessionUserCommandHandler : SessionHandler, IRequestHandler<EditSessionUserCommand, Result<IdentityResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IUserMapper _userMapper;

    public EditSessionUserCommandHandler(IUserRepository userRepository, IJwtService jwtService, IUserMapper userMapper,
        ISessionRepository sessionRepository, ISessionMapper sessionMapper, IAuthorizationService authorizationService)
        : base(sessionRepository, sessionMapper, authorizationService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _userMapper = userMapper;
    }

    public async Task<Result<IdentityResponse>> Handle(EditSessionUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.User;
        if (!user.SessionId.HasValue || !user.UserType.HasValue)
            return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var authResult = await _authorizationService.AuthorizeAsync(user, cancellationToken);
        if (!authResult.IsAuthorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var userEntity = await _userRepository.GetWithSessionsAsync(user.UserId, cancellationToken);
        if (userEntity == null) throw new DatabaseException();

        userEntity.CurrentSession!.Nickname = request.EditSessionUser.Nickname;
        userEntity.CurrentSession!.Symbol = request.EditSessionUser.Symbol;
        await _userRepository.UpdateAsync(cancellationToken);

        var jwt = _jwtService.CreateToken(user);
        if (string.IsNullOrEmpty(jwt.Token)) throw new JwtException("Failed to create token");

        return Result.Ok(_userMapper.MapToLoginResponse(userEntity, jwt));
    }
}
