using Kris.Interface.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public sealed class PositionMapper : IPositionMapper
{
    public UserPositionModel Map(UserPositionEntity entity)
    {
        return new UserPositionModel
        {
            UserId = entity.UserId,
            UserName = entity.SessionUser?.User?.Login,
            Updated = entity.Updated,
            Positions = new List<Kris.Common.Models.GeoSpatialPosition?>
            {
                entity.Position_0,
                entity.Position_1,
                entity.Position_2
            }
        };
    }
}
