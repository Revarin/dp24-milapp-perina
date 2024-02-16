using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public interface IUserRepository : IRepository<UserEntity>
{
    Task<bool> UserExistsAsync(string login, CancellationToken ct);
    Task<UserEntity?> GetWithCurrentSessionAsync(string login, CancellationToken ct);
    Task<UserEntity?> GetWithCurrentSessionAsync(Guid id,  CancellationToken ct);
    Task<UserEntity?> GetWithAllSessionsAsync(Guid id, CancellationToken ct);
}
