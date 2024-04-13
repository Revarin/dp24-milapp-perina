using Kris.Client.Core.Background.Events;

namespace Kris.Client.Core.Background;

public interface IUserPositionsBackgroundHandler : IBackgroundHandler
{
    public event EventHandler<UserPositionsEventArgs> UserPositionsChanged;
}
