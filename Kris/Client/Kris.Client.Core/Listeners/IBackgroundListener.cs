namespace Kris.Client.Core.Listeners;

public interface IBackgroundListener
{
    void StartListening(CancellationToken ct);
}
