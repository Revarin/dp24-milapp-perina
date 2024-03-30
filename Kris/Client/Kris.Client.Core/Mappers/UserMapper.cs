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
            CurrentSession = response.CurrentSession == null
                ? null
                : new UserIdentityEntity.Session
                {
                    Id = response.CurrentSession.Id,
                    Name = response.CurrentSession.Name,
                    UserType = response.CurrentSession.UserType,
                    Nickname = response.CurrentSession.Nickname,
                    Symbol = response.CurrentSession.Symbol
                },
            JoinedSessions = response.JoinedSessions
        };
    }
}
