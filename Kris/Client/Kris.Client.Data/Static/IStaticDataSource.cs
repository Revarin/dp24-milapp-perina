namespace Kris.Client.Data.Static;

public interface IStaticDataSource<T>
{
    IEnumerable<T> Get();
}
