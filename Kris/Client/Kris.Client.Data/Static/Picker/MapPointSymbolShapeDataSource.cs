using Kris.Client.Data.Models.Picker;
using Kris.Common.Enums;

namespace Kris.Client.Data.Static.Picker;

public sealed class MapPointSymbolShapeDataSource : IStaticDataSource<MapPointSymbolShapeItem>
{
    public IEnumerable<MapPointSymbolShapeItem> Get()
    {
        return Enum.GetValues(typeof(MapPointSymbolShape))
            .Cast<MapPointSymbolShape>()
            .Select(value => new MapPointSymbolShapeItem
            {
                Value = value,
                Display = Enum.GetName(typeof(MapPointSymbolShape), value)
            });
    }
}
