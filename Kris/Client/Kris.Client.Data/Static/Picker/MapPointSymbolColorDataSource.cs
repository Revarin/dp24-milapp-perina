using Kris.Client.Data.Models.Picker;
using Kris.Common.Enums;

namespace Kris.Client.Data.Static.Picker;

public sealed class MapPointSymbolColorDataSource : IStaticDataSource<MapPointSymbolColorItem>
{
    public IEnumerable<MapPointSymbolColorItem> Get()
    {
        return Enum.GetValues(typeof(MapPointSymbolColor))
            .Cast<MapPointSymbolColor>()
            .Select(value => new MapPointSymbolColorItem
            { 
                Value = value, 
                Display = Enum.GetName(typeof(MapPointSymbolColor), value)
            });
    }
}
