
namespace Kris.Client.Core.Mappers;

public sealed class MapObjectsMapper : IMapObjectsMapper
{
    public Models.MapPointListModel Map(Interface.Models.MapPointListModel model)
    {
        return new Models.MapPointListModel
        {
            Id = model.Id,
            Name = model.Name,
            Created = model.Created,
            Creator = new Models.UserListModel
            {
                Id = model.User.Id,
                Name = model.User.Name
            },
            Location = new Location(model.Position.Latitude, model.Position.Longitude, model.Position.Altitude),
            Symbol = model.Symbol
        };
    }
}
