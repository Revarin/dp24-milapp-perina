using Kris.Common.Models;
using Kris.Interface.Responses;
using Kris.Server.Core.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public interface IUserMapper
{
    UserEntity Map(CurrentUserModel model);
    CurrentUserModel Map(UserEntity entity);
    LoginResponse MapToLoginResponse(UserEntity entity, JwtToken jwt);
}
