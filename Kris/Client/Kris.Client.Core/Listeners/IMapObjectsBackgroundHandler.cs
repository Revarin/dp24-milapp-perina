using Kris.Client.Core.Listeners.Events;

namespace Kris.Client.Core.Listeners;

public interface IMapObjectsBackgroundHandler : IBackgroundHandler
{
    public event EventHandler<MapObjectsEventArgs> MapObjectsChanged;
}
