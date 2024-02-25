using Kris.Client.Core.Listeners.Events;

namespace Kris.Client.Core.Listeners;

public interface IUserPositionsListener : IBackgroundListener
{
    event EventHandler<UserPositionsEventArgs> PositionsChanged;
}
