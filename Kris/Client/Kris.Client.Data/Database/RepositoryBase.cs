using SQLite;

namespace Kris.Client.Data.Database;

public abstract class RepositoryBase : IRepositoryBase
{
    protected readonly SQLiteConnection _database;

    public RepositoryBase(SQLiteConnection database)
    {
        _database = database;
    }

    public void Dispose()
    {
        _database.Close();
        _database.Dispose();
    }
}
