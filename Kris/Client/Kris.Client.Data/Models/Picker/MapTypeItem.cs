using Kris.Common.Enums;

namespace Kris.Client.Data.Models.Picker;

public sealed class MapTypeItem : IDisplayableItem<KrisMapType>
{
    public string Display { get; init; }
    public KrisMapType Value { get; init; }
}
