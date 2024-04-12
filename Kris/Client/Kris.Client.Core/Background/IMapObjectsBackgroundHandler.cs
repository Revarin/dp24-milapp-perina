using Kris.Client.Core.Background.Events;

namespace Kris.Client.Core.Background;

public interface IMapObjectsBackgroundHandler : IBackgroundHandler
{
    public event EventHandler<MapObjectsEventArgs> MapObjectsChanged;
}
