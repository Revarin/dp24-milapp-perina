using Kris.Client.Core.Listeners.Events;

namespace Kris.Client.Core.Listeners;

public interface ICurrentPositionBackgroundHandler : IBackgroundHandler
{
    public event EventHandler<LocationEventArgs> CurrentPositionChanged;
}
