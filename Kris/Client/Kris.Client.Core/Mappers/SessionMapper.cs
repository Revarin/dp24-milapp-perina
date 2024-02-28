using Kris.Client.Core.Models;
using Kris.Interface.Models;

namespace Kris.Client.Core.Mappers;

public sealed class SessionMapper : ISessionMapper
{
    public SessionListModel Map(SessionModel model)
    {
        return new SessionListModel
        {
            Id = model.Id,
            Name = model.Name,
            Created = model.Created,
            UserCount = model.UserCount
        };
    }
}
