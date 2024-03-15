using Kris.Common.Enums;

namespace Kris.Client.Data.Models.Picker;

public sealed class CoordinateSystemItem : IDisplayableItem<CoordinateSystem>
{
    public string Display { get; init; }
    public CoordinateSystem Value { get; init; }
}
