using Kris.Server.Core.Requests;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public interface IUserMapper
{
    UserEntity Map(RegisterUserCommand command);
}
