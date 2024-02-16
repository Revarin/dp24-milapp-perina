using Kris.Interface.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public sealed class MapObjectMapper : IMapObjectMapper
{
    public MapPointModel MapPoint(MapPointEntity entity)
    {
        return new MapPointModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Type = entity.Type,
            Created = entity.Created,
            Position = entity.Position,
            Symbol = entity.Symbol,
            User = new UserModel
            {
                Id = entity.UserId,
                Name = entity.SessionUser?.User?.Login
            }
        };
    }
}
