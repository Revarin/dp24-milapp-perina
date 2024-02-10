using Kris.Common.Enums;
using Kris.Interface.Models;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
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
            IsActive = entity.IsActive,
            UserCount = entity.Users.Count()
        };
    }
}
