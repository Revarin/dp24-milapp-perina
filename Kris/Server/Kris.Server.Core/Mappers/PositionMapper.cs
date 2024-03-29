using Kris.Common.Models;
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
            UserName = entity.SessionUser!.Nickname,
            Updated = entity.Updated,
            Positions = new List<GeoSpatialPosition?>
            {
                entity.Position_0,
                entity.Position_1,
                entity.Position_2
            }
        };
    }
}
