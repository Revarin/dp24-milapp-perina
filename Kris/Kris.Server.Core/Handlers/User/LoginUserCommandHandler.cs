using FluentResults;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Common.Models;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Models;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.User;

public sealed class LoginUserCommandHandler : UserHandler, IRequestHandler<LoginUserCommand, Result<JwtToken>>
{
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public LoginUserCommandHandler(IPasswordService passwordService, IJwtService jwtService, IUserRepository userRepository, IUserMapper mapper)
        : base(userRepository, mapper)
    {
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task<Result<JwtToken>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var query = await _userRepository.GetAsync(p => p.Login == request.LoginUser.Login, cancellationToken);

        var user = query.FirstOrDefault();
        if (user == null) return Result.Fail(new InvalidCredentialsError());
        if (user.Password == null) throw new Exception("Password missig in db");

        var passwordVerified = _passwordService.VerifyPassword(user.Password, request.LoginUser.Password);
        if (!passwordVerified) return Result.Fail(new InvalidCredentialsError());

        var jwt = _jwtService.CreateToken(user);
        if (string.IsNullOrEmpty(jwt.Token)) throw new JwtException("Failed to create JWT token");

        return Result.Ok(jwt);
    }
}
