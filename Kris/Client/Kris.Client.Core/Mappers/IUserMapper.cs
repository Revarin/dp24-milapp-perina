using Kris.Client.Data.Models;
using Kris.Interface.Responses;

namespace Kris.Client.Core.Mappers;

public interface IUserMapper
{
    UserIdentityTokenEntity Map(LoginResponse response);
}
