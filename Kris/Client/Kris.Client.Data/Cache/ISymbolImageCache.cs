namespace Kris.Client.Data.Cache;

public interface ISymbolImageCache
{
    bool Exists(string fileName);
    ImageSource Load(string fileName);
    void Save(string fileName, Stream data);
}
