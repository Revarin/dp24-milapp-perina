namespace Kris.Client.Data.Cache;

public sealed class SymbolImageCache : ISymbolImageCache
{
    public bool Exists(string fileName)
    {
        return File.Exists(GetFilePath(fileName));
    }

    public ImageSource LoadImageSource(string fileName)
    {
        return ImageSource.FromFile(GetFilePath(fileName));
    }

    public MemoryStream LoadStream(string fileName)
    {
        MemoryStream stream = new MemoryStream();
        using var fileStream = File.OpenRead(GetFilePath(fileName));
        fileStream.CopyTo(stream);
        fileStream.Close();
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
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
