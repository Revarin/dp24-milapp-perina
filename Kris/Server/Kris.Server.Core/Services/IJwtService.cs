using Kris.Common.Enums;
using Kris.Server.Common.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Services;

public interface IJwtService
{
    JwtToken CreateToken(UserEntity user);
    JwtToken CreateToken(UserEntity user, SessionEntity session, UserType userType);
}
