using Kris.Interface.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public sealed class PositionMapper : IPositionMapper
{
    public UserPositionModel Map(UserPositionEntity entity)
    {
        return new UserPositionModel
        {
            UserId = entity.SessionUser!.UserId,
            UserName = entity.SessionUser!.User!.Login,
            Updated = entity.Updated,
            Positions = entity.Positions.ToList(),
        };
    }
}
