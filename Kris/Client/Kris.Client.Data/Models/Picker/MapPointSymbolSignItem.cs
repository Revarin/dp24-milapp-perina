using Kris.Common.Enums;

namespace Kris.Client.Data.Models.Picker;

public sealed class MapPointSymbolSignItem : IDisplayableItem<MapPointSymbolSign>
{
    public string Display { get; init; }
    public MapPointSymbolSign Value { get; init; }
}
