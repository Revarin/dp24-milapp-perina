using SQLite;

namespace Kris.Client.Data.Database;

public sealed class MapTileRepository : RepositoryBase, IMapTileRepository
{
    public MapTileRepository(SQLiteConnection database) : base(database)
    {
    }
}
