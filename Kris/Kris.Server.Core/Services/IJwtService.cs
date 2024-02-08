using Kris.Server.Common.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Services;

public interface IJwtService
{
    JwtToken CreateToken(UserEntity user);
}
