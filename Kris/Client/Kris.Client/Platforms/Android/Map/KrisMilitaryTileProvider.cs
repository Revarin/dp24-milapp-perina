using Android.Gms.Maps.Model;
using Kris.Client.Data.Models.Database;

namespace Kris.Client.Platforms.Map;

public sealed class KrisMilitaryTileProvider : Java.Lang.Object, ITileProvider
{
    private Func<int, int, int, TileEntity> _tileSource;

    public KrisMilitaryTileProvider(Func<int, int, int, TileEntity> tileSource)
    {
        _tileSource = tileSource;
    }

    public Tile GetTile(int x, int y, int zoom)
    {
        var tile = _tileSource(x, y, zoom);
        if (tile == null) return null;
        var s = tile.S == 0 ? 256 : tile.S;
        return new Tile(s, s, tile.Image);
    }
}
