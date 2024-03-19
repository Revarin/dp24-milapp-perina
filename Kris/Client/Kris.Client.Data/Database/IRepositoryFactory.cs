namespace Kris.Client.Data.Database;

public interface IRepositoryFactory
{
    IMapTileRepository CreateMapTileRepository(string databasePath);
}
