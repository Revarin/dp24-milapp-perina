using Kris.Client.Core.Listeners.Events;

namespace Kris.Client.Core.Background;

public interface ICurrentPositionBackgroundHandler : IBackgroundHandler
{
    public event EventHandler<UserPositionEventArgs> CurrentPositionChanged;
}
