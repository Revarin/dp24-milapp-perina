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
            var x = _dbSet.Add(entity);
            _context.SaveChanges();
            return x.Entity;
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
            return;
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
    }
}
