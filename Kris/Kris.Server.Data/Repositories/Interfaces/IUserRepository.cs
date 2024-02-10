using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public interface IUserRepository : IRepository<UserEntity>
{
    Task<bool> UserExistsAsync(string login, CancellationToken ct);
    Task<UserEntity?> GetByLoginAsync(string login, CancellationToken ct);
    Task<UserEntity?> GetByIdAsync(Guid id,  CancellationToken ct);
    Task<UserEntity?> GetByIdWithAllSessionsAsync(Guid id, CancellationToken ct);
}
