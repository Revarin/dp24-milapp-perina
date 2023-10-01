namespace Kris.Client.Core
{
    public interface IGpsService
    {
        event EventHandler<GpsLocationEventArgs> RaiseGpsLocationEvent;
        bool IsListening { get; set; }

        void SetupListener(int millisecondsDelay, int millisecondsTimeout);
        Task StartListeningAsync();
        void StopListening();
        Task<Location> GetGpsLocationAsync(int millisecondsTimeout);
        void CancelRequest();
        Task<Location> GetLastGpsLocationAsync();
        Task<bool> IsGpsEnabled();
    }
}
