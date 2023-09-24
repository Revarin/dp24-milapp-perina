namespace Kris.Server.Data
{
    public interface IRepository<T>
    {
        T Get(int id);
        IQueryable<T> Get();
        IQueryable<T> Get(Func<T, bool> predicate);
        T Insert(T entity);
        T Update(T entity);
        void Delete(T entity);
        void Delete(int id);
    }
}
