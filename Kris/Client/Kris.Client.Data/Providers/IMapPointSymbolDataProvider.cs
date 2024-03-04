using Kris.Client.Data.Models.Picker;

namespace Kris.Client.Data.Providers;

public interface IMapPointSymbolDataProvider
{
    IEnumerable<MapPointSymbolColorItem> GetMapPointSymbolColorItems();
    IEnumerable<MapPointSymbolShapeItem> GetMapPointSymbolShapeItems();
    IEnumerable<MapPointSymbolSignItem> GetMapPointSymbolSignItems();
}
