using Kris.Client.Data.Models;
using Kris.Interface.Responses;

namespace Kris.Client.Core.Mappers;

public sealed class UserMapper : IUserMapper
{
    public UserIdentityTokenEntity Map(LoginResponse response)
    {
        return new UserIdentityTokenEntity
        {
            UserId = response.UserId,
            Login = response.Login,
            SessionId = response.SessionId,
            SessioName = response.SessionName,
            UserType = response.UserType,
            Token = response.Token
        };
    }
}
