using Kris.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data.Repositories;

public abstract class RepositoryBase<T> : IRepository<T> where T : EntityBase
{
    protected DataContext _context { get; set; }
    private DbSet<T> _dbSet => _context.Set<T>();

    public RepositoryBase(DataContext dataContext)
    {
        _context = dataContext;
    }

    public async Task<T?> GetAsync(Guid id, CancellationToken ct)
    {
        return await _dbSet.FindAsync(id, ct);
    }

    public async Task<IEnumerable<T>> GetAsync(CancellationToken ct)
    {
        return await _dbSet.ToListAsync(ct);
    }

    public async Task<IEnumerable<T>> GetAsync(Func<T, bool> predicate, CancellationToken ct)
    {
        return await Task.Run(() => _dbSet.Where(predicate).ToList());
    }

    public async Task<T> InsertAsync(T entity, CancellationToken ct)
    {
        var entry = _dbSet.Add(entity);
        await _context.SaveChangesAsync(ct);
        return entry.Entity;
    }

    public async Task<bool> UpdateAsync(T entity, CancellationToken ct)
    {
        var entityExists = await _dbSet.AnyAsync(p => p.Id == entity.Id, ct);
        if (!entityExists) return false;

        //_dbSet.Update(entity);
        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var entity = await _dbSet.FindAsync(id, ct);
        if (entity == null) return false;

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task ForceSaveAsync(CancellationToken ct)
    {
        await _context.SaveChangesAsync(ct);
    }
}
