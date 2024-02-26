using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public sealed class UserSettingsRepository : RepositoryBase<UserSettingsEntity>, IUserSettingsRepository
{
    public UserSettingsRepository(DataContext dataContext) : base(dataContext)
    {
    }
}
