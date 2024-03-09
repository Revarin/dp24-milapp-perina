namespace Kris.Client.Core.Listeners;

public interface IBackgroundLoop
{
    public bool IsRunning { get; }
    public bool ReloadSettings { get; set; }

    void RegisterService(IBackgroundHandler service);
    void UnregisterService(IBackgroundHandler service);
    void ClearServices();
    Task Start(CancellationToken ct);
}
