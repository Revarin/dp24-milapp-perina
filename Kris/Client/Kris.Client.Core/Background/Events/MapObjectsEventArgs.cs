using Kris.Client.Core.Models;

namespace Kris.Client.Core.Background.Events;

public sealed class MapObjectsEventArgs : EventArgs
{
    public IEnumerable<MapPointListModel> MapPoints { get; init; }
    public DateTime Loaded { get; init; }

    public MapObjectsEventArgs(IEnumerable<MapPointListModel> mapPoints, DateTime loaded)
    {
        MapPoints = mapPoints;
        Loaded = loaded;
    }
}
