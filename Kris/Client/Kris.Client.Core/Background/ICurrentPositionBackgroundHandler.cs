using Kris.Client.Core.Background.Events;

namespace Kris.Client.Core.Background;

public interface ICurrentPositionBackgroundHandler : IBackgroundHandler
{
    public event EventHandler<UserPositionEventArgs> CurrentPositionChanged;

    public new bool IsRunning { get; set; }
}
