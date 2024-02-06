using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public interface IUserRepository : IRepository<UserEntity>
{
    Task<bool> UserExistsAsync(string login, CancellationToken ct);
}
