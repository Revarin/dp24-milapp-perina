
namespace Kris.Client.Core.Mappers;

public sealed class MapObjectsMapper : IMapObjectsMapper
{
    public Models.MapPointModel Map(Interface.Models.MapPointModel model)
    {
        return new Models.MapPointModel
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
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
