using FluentResults;
using Kris.Common.Models;
using Kris.Interface.Responses;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.User;

public sealed class LoginUserCommandHandler : UserHandler, IRequestHandler<LoginUserCommand, Result<LoginResponse>>
{
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public LoginUserCommandHandler(IPasswordService passwordService, IJwtService jwtService, IUserRepository userRepository, IUserMapper mapper)
        : base(userRepository, mapper)
    {
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task<Result<LoginResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetWithCurrentSessionAsync(request.LoginUser.Login, cancellationToken);
        if (user == null) return Result.Fail(new InvalidCredentialsError());

        var passwordVerified = _passwordService.VerifyPassword(user.Password, request.LoginUser.Password);
        if (!passwordVerified) return Result.Fail(new InvalidCredentialsError());

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
