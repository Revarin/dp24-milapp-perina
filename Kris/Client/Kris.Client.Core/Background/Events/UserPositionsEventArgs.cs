using Kris.Client.Core.Models;

namespace Kris.Client.Core.Background.Events;

public sealed class UserPositionsEventArgs : EventArgs
{
    public IEnumerable<UserPositionModel> Positions { get; init; }
    public DateTime Loaded { get; init; }

    public UserPositionsEventArgs(IEnumerable<UserPositionModel> positions, DateTime loaded)
    {
        Positions = positions;
        Loaded = loaded;
    }
}
