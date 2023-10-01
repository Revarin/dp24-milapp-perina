namespace Kris.Client.Core
{
    public interface IGpsService
    {
        event EventHandler<GpsLocationEventArgs> RaiseGpsLocationEvent;
        bool IsListening { get; set; }

        Task StartListeningAsync(int delayMiliseconds, int timeoutMiliseconds);
        Task StopListening();
        Task<Location> GetGpsLocationAsync(int millisecondsTimeout);
        Task<Location> GetLastGpsLocationAsync();
        Task<bool> IsGpsEnabled(int timeoutSeconds);
    }
}
