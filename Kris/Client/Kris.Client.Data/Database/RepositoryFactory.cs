using SQLite;

namespace Kris.Client.Data.Database;

public sealed class RepositoryFactory : IRepositoryFactory
{
    public IMapTileRepository CreateMapTileRepository(string databasePath)
    {
        return new MapTileRepository(new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadOnly));
    }
}
