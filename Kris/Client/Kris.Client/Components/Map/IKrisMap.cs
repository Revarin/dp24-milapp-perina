using IMap = Microsoft.Maui.Maps.IMap;

namespace Kris.Client.Components.Map;

public interface IKrisMap : IMap
{
    public KrisMapStyle KrisMapStyle { get; set; }

    void LongClicked(Location location);
    void MoveStarted();
}
