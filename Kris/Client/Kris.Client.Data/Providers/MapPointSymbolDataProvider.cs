using Kris.Client.Data.Models.Picker;
using Kris.Client.Data.Static;
using Kris.Client.Data.Static.Picker;

namespace Kris.Client.Data.Providers;

public sealed class MapPointSymbolDataProvider : IMapPointSymbolDataProvider
{
    private readonly IStaticDataSource<MapPointSymbolColorItem> _mapPointSymbolColorItems;
    private readonly IStaticDataSource<MapPointSymbolShapeItem> _mapPointSymbolShapeItems;
    private readonly IStaticDataSource<MapPointSymbolSignItem> _mapPointSymbolSignItems;

    public MapPointSymbolDataProvider()
    {
        _mapPointSymbolColorItems = new MapPointSymbolColorDataSource();
        _mapPointSymbolShapeItems = new MapPointSymbolShapeDataSource();
        _mapPointSymbolSignItems = new MapPointSymbolSignDataSource();
    }

    public IEnumerable<MapPointSymbolColorItem> GetMapPointSymbolColorItems()
    {
        return _mapPointSymbolColorItems.Get();
    }

    public IEnumerable<MapPointSymbolShapeItem> GetMapPointSymbolShapeItems()
    {
        return _mapPointSymbolShapeItems.Get();
    }

    public IEnumerable<MapPointSymbolSignItem> GetMapPointSymbolSignItems()
    {
        return _mapPointSymbolSignItems.Get();
    }
}
