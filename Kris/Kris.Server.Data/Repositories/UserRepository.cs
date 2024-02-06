using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public sealed class UserRepository : RepositoryBase<UserEntity>, IUserRepository
{
    public UserRepository(DataContext dataContext) : base(dataContext)
    {
    }
}
