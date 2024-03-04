using IMap = Microsoft.Maui.Maps.IMap;

namespace Kris.Client.Components.Map;

public interface IKrisMap : IMap
{
    void LongClicked(Location location);
}
