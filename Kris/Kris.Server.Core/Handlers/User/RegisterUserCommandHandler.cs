using FluentResults;
using MediatR;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Data.Repositories;
using Kris.Server.Data.Models;
using Kris.Server.Core.Services;

namespace Kris.Server.Core.Handlers.User;

public sealed class RegisterUserCommandHandler : UserHandler, IRequestHandler<RegisterUserCommand, Result>
{
    private readonly IPasswordService _passwordService;

    public RegisterUserCommandHandler(IPasswordService passwordService, IUserRepository userRepository, IUserMapper mapper)
        : base(userRepository, mapper)
    {
        _passwordService = passwordService;
    }

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _userRepository.UserExistsAsync(request.RegisterUser.Login, cancellationToken);
        if (userExists) return Result.Fail(new EntityExistsError("User", request.RegisterUser.Login));

        var user = new UserEntity
        {
            Login = request.RegisterUser.Login,
            Password = _passwordService.HashPassword(request.RegisterUser.Password),
            Created = DateTime.UtcNow
        };
        await _userRepository.InsertAsync(user, cancellationToken);

        return Result.Ok();
    }
}
