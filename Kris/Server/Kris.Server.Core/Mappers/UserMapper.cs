using Kris.Common.Models;
using Kris.Interface.Responses;
using Kris.Server.Core.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public sealed class UserMapper : IUserMapper
{
    public CurrentUserModel Map(UserEntity entity)
    {
        var user = new CurrentUserModel
        {
            UserId = entity.Id,
            Login = entity.Login,
            SessionId = entity.CurrentSession?.SessionId,
            UserType = entity.CurrentSession?.UserType,
            SessionName = entity.CurrentSession?.Session?.Name
        };

        return user;
    }

    public LoginResponse MapToLoginResponse(UserEntity entity, JwtToken jwt)
    {
        return new LoginResponse
        {
            UserId = entity.Id,
            Login = entity.Login,
            Token = jwt.Token,
            JoinedSessions = entity.AllSessions.Select(session => session.SessionId),
            CurrentSession = entity.CurrentSession?.Session == null
                ? null
                : new LoginResponse.Session
                {
                    Id = entity.CurrentSession.SessionId,
                    Name = entity.CurrentSession.Session.Name,
                    UserType = entity.CurrentSession.UserType
                }
        };
    }
}
