using Kris.Client.Data.Models.Database;
using SQLite;

namespace Kris.Client.Data.Database;

public sealed class MapTileRepository : RepositoryBase, IMapTileRepository
{
    public MapTileRepository(SQLiteConnection database) : base(database)
    {
    }

    public InfoEntity GetInfo()
    {
        return _database.Table<InfoEntity>().FirstOrDefault();
    }

    public TileEntity GetTile(int x, int y, int z)
    {
        return _database.Table<TileEntity>().FirstOrDefault(tile => tile.X == x && tile.Y == y);
    }
}
