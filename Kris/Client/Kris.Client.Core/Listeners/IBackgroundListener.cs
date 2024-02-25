using Kris.Client.Common.Events;

namespace Kris.Client.Core.Listeners;

public interface IBackgroundListener
{
    event EventHandler<ResultEventArgs> ErrorOccured;
    bool IsListening { get; }
    Task StartListening(CancellationToken ct);
}
