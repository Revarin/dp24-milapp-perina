using Kris.Client.Core.Listeners.Events;

namespace Kris.Client.Core.Listeners;

public interface IUserPositionsBackgroundHandler : IBackgroundHandler
{
    public event EventHandler<UserPositionsEventArgs> UserPositionsChanged;
}
