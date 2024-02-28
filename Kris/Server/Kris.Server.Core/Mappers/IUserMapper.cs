using Kris.Common.Models;
using Kris.Interface.Responses;
using Kris.Server.Core.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public interface IUserMapper
{
    CurrentUserModel Map(UserEntity entity);
    IdentityResponse MapToLoginResponse(UserEntity entity, JwtToken jwt);
}
