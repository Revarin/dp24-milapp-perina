﻿using FluentResults;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Models;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.User;

public sealed class LoginUserCommandHandler : UserHandler, IRequestHandler<LoginUserCommand, Result>
{
    private readonly IPasswordService<UserEntity> _passwordService;

    public LoginUserCommandHandler(IPasswordService<UserEntity> passwordService, IUserRepository userRepository, IUserMapper mapper)
        : base(userRepository, mapper)
    {
        _passwordService = passwordService;
    }

    public async Task<Result> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var query = await _userRepository.GetAsync(p => p.Login == request.LoginUser.Login, cancellationToken);

        var user = query.FirstOrDefault();
        if (user == null) return Result.Fail(new InvalidCredentialsError());

        var passwordVerified = _passwordService.VerifyPassword(user, request.LoginUser.Password);
        if (!passwordVerified) return Result.Fail(new InvalidCredentialsError());

        return Result.Ok();
    }
}
