using Kris.Common.Enums;
using Kris.Server.Common.Models;
using Kris.Server.Core.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Services;

public interface IJwtService
{
    JwtToken CreateToken(UserEntity user);
    JwtToken CreateToken(UserModel user, SessionEntity session, UserType userType = UserType.Basic);
}
