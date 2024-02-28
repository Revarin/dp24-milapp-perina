using Kris.Client.Data.Models;
using Kris.Interface.Responses;

namespace Kris.Client.Core.Mappers;

public sealed class UserMapper : IUserMapper
{
    public UserIdentityTokenEntity Map(IdentityResponse response)
    {
        return new UserIdentityTokenEntity
        {
            UserId = response.UserId,
            Login = response.Login,
            Token = response.Token,
            SessionId = response.CurrentSession?.Id,
            SessionName = response.CurrentSession?.Name,
            UserType = response.CurrentSession?.UserType,
            JoinedSessions = response.JoinedSessions
        };
    }
}
