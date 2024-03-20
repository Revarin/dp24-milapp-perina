namespace Kris.Client.Data.Cache;

public interface IFileStore
{
    bool CacheExists(string fileName);
    bool DataExists(string fileName);
    string SaveToCache(string fileName, Stream data);
    string SaveToData(string fileName, Stream data);
}
