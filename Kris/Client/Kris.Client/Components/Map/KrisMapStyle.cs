namespace Kris.Client.Components.Map;

public sealed class KrisMapStyle
{
    public string JsonStyle { get; init; }

    public KrisMapStyle(string jsonStyle)
    {
        JsonStyle = jsonStyle;
    }
}
