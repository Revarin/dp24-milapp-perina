using Kris.Interface.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public sealed class MapObjectMapper : IMapObjectMapper
{
    public MapPointDetailModel MapPointDetail(MapPointEntity entity)
    {
        return new MapPointDetailModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Type = entity.Type,
            Description = entity.Description,
            Created = entity.Created,
            Position = entity.Position,
            Symbol = entity.Symbol,
            User = new UserModel
            {
                Id = entity.SessionUser!.UserId,
                Name = entity.SessionUser!.User?.Login
            }
        };
    }

    public MapPointListModel MapPointList(MapPointEntity entity)
    {
        return new MapPointListModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Type = entity.Type,
            Created = entity.Created,
            Position = entity.Position,
            Symbol = entity.Symbol,
            User = new UserModel
            {
                Id = entity.SessionUser!.UserId,
                Name = entity.SessionUser!.User?.Login
            }
        };
    }
}
