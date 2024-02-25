using Kris.Server.Data.Models;

namespace Kris.Server.Data.Repositories;

public interface IRepository<T> where T : EntityBase
{
    Task<T?> GetAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<T>> GetAsync(CancellationToken ct);
    Task<IEnumerable<T>> GetAsync(Func<T, bool> predicate, CancellationToken ct);
    Task<T> InsertAsync(T entity, CancellationToken ct);
    Task UpdateAsync(CancellationToken ct);
    Task DeleteAsync(T entity, CancellationToken ct);
}
