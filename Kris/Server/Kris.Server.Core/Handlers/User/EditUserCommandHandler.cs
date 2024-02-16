using FluentResults;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Common.Models;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.User;

public sealed class EditUserCommandHandler : UserHandler, IRequestHandler<EditUserCommand, Result<JwtToken>>
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

    public async Task<Result<JwtToken>> Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetWithCurrentSessionAsync(request.User.Id, cancellationToken);
        if (user == null) throw new NullableException();
        if (user.Login != request.User.Login || user.CurrentSessionId != request.User.SessionId)
            return Result.Fail(new UnauthorizedError("Invalid token"));

        user.Login = request.EditUser.Login;
        user.Password = _passwordService.HashPassword(request.EditUser.Password);
        var updated = await _userRepository.UpdateAsync(user, cancellationToken);
        if (!updated) throw new DatabaseException("Failed to update user");

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

        return Result.Ok(jwt);
    }
}
