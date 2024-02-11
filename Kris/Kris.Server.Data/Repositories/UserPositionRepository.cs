using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public sealed class UserPositionRepository : RepositoryBase<UserPositionEntity>, IUserPositionRepository
{
    public UserPositionRepository(DataContext dataContext) : base(dataContext)
    {
    }
}
