using Kris.Client.Data.Models.Picker;
using Kris.Common.Enums;

namespace Kris.Client.Data.Static.Picker;

public sealed class MapTypeDataSource : IStaticDataSource<MapTypeItem>
{
    private List<MapTypeItem> _source = new List<MapTypeItem>
    {
        new MapTypeItem { Value = KrisMapType.StreetDark, Display = "Dark" },
        new MapTypeItem { Value = KrisMapType.StreetLight, Display = "Light" },
        new MapTypeItem { Value = KrisMapType.Satelite, Display = "Satelite" },
        new MapTypeItem { Value = KrisMapType.Custom, Display = "Custom" }
    };

    public IEnumerable<MapTypeItem> Get()
    {
        return _source;
    }
}
