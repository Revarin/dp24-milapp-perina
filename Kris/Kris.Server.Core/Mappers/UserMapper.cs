using Kris.Server.Core.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public sealed class UserMapper : IUserMapper
{
    public UserMapper()
    {
    }

    public CurrentUserModel Map(UserEntity entity)
    {
        var user = new CurrentUserModel
        {
            Id = entity.Id,
            Login = entity.Login
        };

        return user;
    }

    public UserEntity Map(CurrentUserModel model)
    {
        return new UserEntity
        {
            Id = model.Id,
            Login = model.Login,
            Password = string.Empty,
            Created = DateTime.MinValue
        };
    }
}
