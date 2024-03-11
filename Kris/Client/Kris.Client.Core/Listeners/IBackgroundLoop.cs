namespace Kris.Client.Core.Listeners;

public interface IBackgroundLoop
{
    public bool IsRunning { get; }
    public bool ReloadSettings { get; set; }

    void RegisterHandler(IBackgroundHandler handler);
    void UnregisterHandler(IBackgroundHandler handler);
    void ClearHandlers();
    void ResetHandlers();
    Task Start(CancellationToken ct);
}
