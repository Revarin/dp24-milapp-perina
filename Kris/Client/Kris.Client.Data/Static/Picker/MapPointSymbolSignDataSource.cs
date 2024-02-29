using Kris.Client.Data.Models.Picker;
using Kris.Common.Enums;

namespace Kris.Client.Data.Static.Picker;

public sealed class MapPointSymbolSignDataSource : IStaticDataSource<MapPointSymbolSignItem>
{
    public IEnumerable<MapPointSymbolSignItem> Get()
    {
        return Enum.GetValues(typeof(MapPointSymbolSign))
            .Cast<MapPointSymbolSign>()
            .Select(value => new MapPointSymbolSignItem
            {
                Value = value,
                Display = Enum.GetName(typeof(MapPointSymbolSign), value)
            });
    }
}
