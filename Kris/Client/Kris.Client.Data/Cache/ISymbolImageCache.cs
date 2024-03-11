namespace Kris.Client.Data.Cache;

public interface ISymbolImageCache
{
    bool Exists(string fileName);
    ImageSource LoadImageSource(string fileName);
    MemoryStream LoadStream(string fileName);
    void Save(string fileName, Stream data);
}
