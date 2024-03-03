namespace Kris.Client.Data.Cache;

public sealed class SymbolImageCache : ISymbolImageCache
{
    public bool Exists(string fileName)
    {
        return File.Exists(GetFilePath(fileName));
    }

    public ImageSource Load(string fileName)
    {
        return ImageSource.FromFile(GetFilePath(fileName));
    }

    public void Save(string fileName, Stream data)
    {
        using var fileStream = File.OpenWrite(GetFilePath(fileName));
        data.Seek(0, SeekOrigin.Begin);
        data.CopyTo(fileStream);
        fileStream.Close();
        data.Seek(0, SeekOrigin.Begin);
    }

    private string GetFilePath(string fileName) => Path.Combine(FileSystem.Current.CacheDirectory, fileName);
}
