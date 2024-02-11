using Kris.Interface.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public interface IPositionMapper
{
    UserPositionModel Map(UserPositionEntity entity);
}
