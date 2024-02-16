using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public interface IRepository<T> where T : EntityBase
{
    Task<T?> GetAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<T>> GetAsync(CancellationToken ct);
    Task<IEnumerable<T>> GetAsync(Func<T, bool> predicate, CancellationToken ct);
    Task<T> InsertAsync(T entity, CancellationToken ct);
    Task<bool> UpdateAsync(T entity, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
    Task ForceSaveAsync(CancellationToken ct);
}
