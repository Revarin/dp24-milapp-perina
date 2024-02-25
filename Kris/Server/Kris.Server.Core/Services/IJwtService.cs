using Kris.Common.Models;
using Kris.Server.Core.Models;

namespace Kris.Server.Core.Services;

public interface IJwtService
{
    JwtToken CreateToken(CurrentUserModel user);
}
