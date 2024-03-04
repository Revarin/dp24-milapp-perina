using Kris.Common.Enums;

namespace Kris.Client.Data.Models.Picker;

public sealed class MapPointSymbolShapeItem : IDisplayableItem<MapPointSymbolShape>
{
    public string Display { get; init; }
    public MapPointSymbolShape Value { get; init; }
}
