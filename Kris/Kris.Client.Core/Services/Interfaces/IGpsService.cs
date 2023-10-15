namespace Kris.Client.Core
{
    public interface IGpsService
    {
        event EventHandler<GpsLocationEventArgs> RaiseGpsLocationEvent;
        bool IsListening { get; set; }

        void StartListening(int delayMiliseconds, int timeoutMiliseconds);
        void StopListening();
        Task<Location> GetGpsLocationAsync(int millisecondsTimeout);
        Task<Location> GetLastGpsLocationAsync();
        Task<bool> IsGpsEnabled(int timeoutSeconds);
    }
}
