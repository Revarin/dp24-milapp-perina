using Kris.Common.Enums;

namespace Kris.Client.Components.Map;

public sealed class KrisMapStyle
{
    public KrisMapType KrisMapType { get; init; }
    public string JsonStyle { get; init; }

    public KrisMapStyle(KrisMapType krisMapType, string jsonStyle)
    {
        KrisMapType = krisMapType;
        JsonStyle = jsonStyle;
    }
}
