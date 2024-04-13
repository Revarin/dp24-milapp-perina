using Kris.Client.Common.Events;

namespace Kris.Client.Core.Background;

public interface IBackgroundHandler
{
    public event EventHandler<ResultEventArgs> ErrorOccured;

    public bool IsRunning { get; }
    public bool ReloadSettings { get; set; }
    public TimeSpan Interval { get; }

    Task StartLoopAsync(CancellationToken ct);
    Task ExecuteAsync(CancellationToken ct);
}
