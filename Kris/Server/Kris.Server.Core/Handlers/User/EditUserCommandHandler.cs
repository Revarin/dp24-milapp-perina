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

public sealed class EditUserCommandHandler : UserHandler, IRequestHandler<EditUserCommand, Result<IdentityResponse>>
{
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public EditUserCommandHandler(IPasswordService passwordService, IJwtService jwtService,
        IUserRepository userRepository, IUserMapper mapper)
        : base(userRepository, mapper)
    {
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task<Result<IdentityResponse>> Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetWithSessionsAsync(request.User.UserId, cancellationToken);
        if (user == null) throw new NullableException();
        if (user.Login != request.User.Login) return Result.Fail(new UnauthorizedError("Invalid token"));

        var passwordVerified = _passwordService.VerifyPassword(user.Password, request.EditUser.Password);
        if (!passwordVerified) return Result.Fail(new InvalidCredentialsError());

        user.Password = _passwordService.HashPassword(request.EditUser.NewPassword);
        await _userRepository.UpdateAsync(cancellationToken);

        var jwt = _jwtService.CreateToken(_userMapper.Map(user));
        if (string.IsNullOrEmpty(jwt.Token)) throw new JwtException("Failed to create token");

        return Result.Ok(_userMapper.MapToLoginResponse(user, jwt));
    }
}
