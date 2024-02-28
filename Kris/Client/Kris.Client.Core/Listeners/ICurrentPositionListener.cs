using Kris.Client.Core.Listeners.Events;

namespace Kris.Client.Core.Listeners;

public interface ICurrentPositionListener : IBackgroundListener
{
    event EventHandler<LocationEventArgs> PositionChanged;
}
