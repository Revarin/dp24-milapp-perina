using Kris.Client.Core.Models;

namespace Kris.Client.Core.Listeners.Events;

public sealed class UserPositionEventArgs : EventArgs
{
    public UserPositionModel Position { get; init; }

    public UserPositionEventArgs(UserPositionModel positions)
    {
        Position = positions;
    }
}
