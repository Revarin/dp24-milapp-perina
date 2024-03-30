namespace Kris.Client.Core.Mappers;

public sealed class SessionMapper : ISessionMapper
{
    public Models.SessionListModel Map(Interface.Models.SessionListModel model)
    {
        return new Models.SessionListModel
        {
            Id = model.Id,
            Name = model.Name,
            Created = model.Created,
            UserCount = model.UserCount
        };
    }
}
