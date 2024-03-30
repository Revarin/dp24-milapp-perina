using Kris.Interface.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public sealed class SessionMapper : ISessionMapper
{
    public SessionMapper()
    {
    }

    public SessionDetailModel MapDetail(SessionEntity entity, Guid userId)
    {
        var sessionUser = entity.Users.First(sessionUser => sessionUser.UserId == userId);

        return new SessionDetailModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Created = entity.Created,
            UserName = sessionUser.Nickname,
            UserSymbol = sessionUser.Symbol
        };
    }

    public SessionListModel MapList(SessionEntity entity)
    {
        return new SessionListModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Created = entity.Created,
            UserCount = entity.Users.Count(),
        };
    }
}
