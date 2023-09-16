namespace Kris.Client.Common
{
    public interface IGpsService
    {
        event EventHandler<GpsLocationEventArgs> RaiseGpsLocationEvent;
        bool IsListening { get; set; }

        Task StartListeningAsync(int millisecondsDelay, int millisecondsTimeout);
        void StopListening();
        Task<Location> GetGpsLocationAsync(int millisecondsTimeout);
        void CancelRequest();
    }
}
