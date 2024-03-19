namespace Kris.Client.Data.Cache;

public sealed class FileStore : IFileStore
{
    public bool CacheExists(string fileName)
    {
        return File.Exists(GetCacheFilePath(fileName));
    }

    public bool DataExists(string fileName)
    {
        return File.Exists(GetDataFilePath(fileName));
    }

    public string SaveToCache(string fileName, Stream data)
    {
        using var fileStream = File.OpenWrite(GetCacheFilePath(fileName));
        data.Seek(0, SeekOrigin.Begin);
        data.CopyTo(fileStream);
        fileStream.Close();
        data.Seek(0, SeekOrigin.Begin);
        return GetCacheFilePath(fileName);
    }

    public string SaveToData(string fileName, Stream data)
    {
        using var fileStream = File.OpenWrite(GetDataFilePath(fileName));
        data.Seek(0, SeekOrigin.Begin);
        data.CopyTo(fileStream);
        fileStream.Close();
        data.Seek(0, SeekOrigin.Begin);
        return GetDataFilePath(fileName);
    }

    private string GetCacheFilePath(string fileName) => Path.Combine(FileSystem.Current.CacheDirectory, fileName);

    private string GetDataFilePath(string fileName) => Path.Combine(FileSystem.Current.AppDataDirectory, fileName);

}
