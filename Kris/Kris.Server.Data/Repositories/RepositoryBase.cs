using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected DataContext _context { get; set; }
        private DbSet<T> _dbSet
        {
            get => _context.Set<T>();
        }

        public RepositoryBase(DataContext dataContext)
        {
            _context = dataContext;
        }

        public T Get(int id)
        {
            return _dbSet.Find(id);
        }

        public IQueryable<T> Get()
        {
            return _dbSet;
        }

        public IQueryable<T> Get(Func<T, bool> predicate)
        {
            return _dbSet.Where(predicate).AsQueryable();
        }

        public T Insert(T entity)
        {
            return _context.Add(entity).Entity;
        }

        public T Update(T entity)
        {
            return _context.Update(entity).Entity;
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public void Delete(int id)
        {
            var entity = _context.Find<T>(id);
            _context.Remove(entity);
        }
    }
}
