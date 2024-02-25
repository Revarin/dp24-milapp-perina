using Kris.Interface.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public sealed class SessionMapper : ISessionMapper
{
    public SessionMapper()
    {
    }

    public SessionModel Map(SessionEntity entity)
    {
        return new SessionModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Created = entity.Created,
            UserCount = entity.Users.Count()
        };
    }
}
