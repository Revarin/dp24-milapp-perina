using Kris.Client.Data.Models.Database;
using SQLite;

namespace Kris.Client.Data.Database;

public abstract class RepositoryBase : IRepositoryBase
{
    protected readonly SQLiteConnection _database;

    public RepositoryBase(SQLiteConnection database)
    {
        _database = database;
    }

    public bool IsDbSchemaValid()
    {
        var infoTable = _database.GetTableInfo("info");
        if (infoTable.Count == 0) return false;
        if (!IsTableSchemaValid<InfoEntity>(infoTable)) return false;

        var tilesTable = _database.GetTableInfo("tiles");
        if (tilesTable.Count == 0) return false;
        if (!IsTableSchemaValid<TileEntity>(tilesTable)) return false;

        return true;
    }

    private bool IsTableSchemaValid<T>(List<SQLiteConnection.ColumnInfo> table) where T : class
    {
        foreach(var col in typeof(T).GetProperties())
        {
            if (!table.Any(c => c.Name == col.Name.ToLower())) return false;
        }
        return true;
    }

    public void Dispose()
    {
        _database.Close();
        _database.Dispose();
    }
}
