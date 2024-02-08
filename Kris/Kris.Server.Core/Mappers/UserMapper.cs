using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public sealed class UserMapper : IUserMapper
{
    private readonly IPasswordService<UserEntity> _passwordService;

    public UserMapper(IPasswordService<UserEntity> passwordService)
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

        user.Password = _passwordService.HashPassword(user, command.RegisterUser.Password);

        return user;
    }
}
