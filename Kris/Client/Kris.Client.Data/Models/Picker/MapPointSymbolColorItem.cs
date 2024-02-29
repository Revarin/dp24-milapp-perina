using Kris.Common.Enums;

namespace Kris.Client.Data.Models.Picker;

public sealed class MapPointSymbolColorItem : IDisplayableItem<MapPointSymbolColor>
{
    public string Display { get; init; }
    public MapPointSymbolColor Value { get; init; }
}
