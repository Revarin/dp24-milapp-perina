using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public sealed class MapPointRepository : RepositoryBase<MapPointEntity>, IMapPointRepository
{
    public MapPointRepository(DataContext dataContext) : base(dataContext)
    {
    }
}
