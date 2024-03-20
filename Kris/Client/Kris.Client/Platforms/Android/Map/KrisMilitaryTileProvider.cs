using Android.Gms.Maps.Model;
using Android.Graphics;
using Kris.Client.Data.Models.Database;

namespace Kris.Client.Platforms.Map;

public sealed class KrisMilitaryTileProvider : Java.Lang.Object, ITileProvider
{
    private const int DefaultZoomLevel = 15;
    private const int DefaultScale = 256;

    private Func<int, int, int, TileEntity> _tileSource;

    private enum TilePosition
    {
        TopLeft,
        TopRight,
        BottonLeft,
        BottomRight
    }

    public KrisMilitaryTileProvider(Func<int, int, int, TileEntity> tileSource)
    {
        _tileSource = tileSource;
    }

    public Tile GetTile(int x, int y, int zoom)
    {
        if (zoom < DefaultZoomLevel)
        {
            var sx = x * 2;
            var sy = y * 2;

            var topLeftTile = GetTile(sx, sy, zoom + 1);
            var topRightTile = GetTile(sx + 1, sy, zoom + 1);
            var bottomLeftTile = GetTile(sx, sy + 1, zoom + 1);
            var bottomRightTile = GetTile(sx + 1, sy + 1, zoom + 1);

            if (topLeftTile == null || topRightTile == null || bottomLeftTile == null || bottomRightTile == null) return null;

            var image = Bitmap.CreateBitmap(DefaultScale, DefaultScale, Bitmap.Config.Argb8888);
            var canvas = new Canvas(image);
            var paint = new Android.Graphics.Paint
            {
                AntiAlias = true,
            };

            DrawTile(canvas, paint, BitmapFactory.DecodeByteArray(topLeftTile.Data.ToArray(), 0, topLeftTile.Data.Count), TilePosition.TopLeft);
            DrawTile(canvas, paint, BitmapFactory.DecodeByteArray(topRightTile.Data.ToArray(), 0, topRightTile.Data.Count), TilePosition.TopRight);
            DrawTile(canvas, paint, BitmapFactory.DecodeByteArray(bottomLeftTile.Data.ToArray(), 0, bottomLeftTile.Data.Count), TilePosition.BottonLeft);
            DrawTile(canvas, paint, BitmapFactory.DecodeByteArray(bottomRightTile.Data.ToArray(), 0, bottomRightTile.Data.Count), TilePosition.BottomRight);

            var byteStream = new MemoryStream();
            image.Compress(Bitmap.CompressFormat.Jpeg, 95, byteStream);
            return new Tile(DefaultScale, DefaultScale, byteStream.ToArray());
        }
        else
        {
            var tile = _tileSource(x, y, zoom);
            if (tile == null) return null;
            return new Tile(DefaultScale, DefaultScale, tile.Image);
        }
    }

    private void DrawTile(Canvas canvas, Android.Graphics.Paint paint, Bitmap tile, TilePosition position)
    {
        var matrix = new Matrix();
        matrix.PostScale(0.5f, 0.5f);

        switch (position)
        {
            case TilePosition.TopLeft:
                break;
            case TilePosition.TopRight:
                matrix.PostTranslate(DefaultScale * 0.5f, 0f);
                break;
            case TilePosition.BottonLeft:
                matrix.PostTranslate(0f, DefaultScale * 0.5f);
                break;
            case TilePosition.BottomRight:
                matrix.PostTranslate(DefaultScale * 0.5f, DefaultScale * 0.5f);
                break;
        }

        canvas.DrawBitmap(tile, matrix, paint);
    }
}
