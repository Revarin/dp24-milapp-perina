using Kris.Client.Core.Models;

namespace Kris.Client.Core.Listeners.Events;

public sealed class MapObjectsEventArgs : EventArgs
{
    public IEnumerable<MapPointModel> MapPoints { get; init; }
    public DateTime Loaded { get; init; }

    public MapObjectsEventArgs(IEnumerable<MapPointModel> mapPoints, DateTime loaded)
    {
        MapPoints = mapPoints;
        Loaded = loaded;
    }
}
