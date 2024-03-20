using Kris.Client.Data.Models.Database;

namespace Kris.Client.Data.Database;

public interface IMapTileRepository : IRepositoryBase
{
    TileEntity GetTile(int x, int y, int z);
    InfoEntity GetInfo();
}
