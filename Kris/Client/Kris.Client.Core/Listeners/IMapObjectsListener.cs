using Kris.Client.Core.Listeners.Events;

namespace Kris.Client.Core.Listeners;

public interface IMapObjectsListener : IBackgroundListener
{
    event EventHandler<MapObjectsEventArgs> MapObjectsChanged;
}
