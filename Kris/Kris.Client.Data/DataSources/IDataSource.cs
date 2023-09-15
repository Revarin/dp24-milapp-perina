namespace Kris.Client.Data
{
    public interface IDataSource<T>
    {
        IEnumerable<T> Get();
    }
}
