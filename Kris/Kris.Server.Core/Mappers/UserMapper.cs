﻿using Kris.Server.Core.Models;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public sealed class UserMapper : IUserMapper
{
    private readonly IPasswordService _passwordService;

    public UserMapper(IPasswordService passwordService)
    {
        _passwordService = passwordService;
    }

    public UserEntity Map(RegisterUserCommand command)
    {
        var user = new UserEntity
        {
            Login = command.RegisterUser.Login,
            Created = DateTime.UtcNow
        };

        user.Password = _passwordService.HashPassword(command.RegisterUser.Password);

        return user;
    }

    public UserModel Map(UserEntity entity)
    {
        var user = new UserModel
        {
            Id = entity.Id,
            Login = entity.Login
        };

        return user;
    }
}