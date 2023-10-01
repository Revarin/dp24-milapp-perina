using Kris.Client.Common;

namespace Kris.Client.Core
{
    public class GpsService : IGpsService
    {
        public event EventHandler<GpsLocationEventArgs> RaiseGpsLocationEvent;

        public bool IsListening { get; set; }

        private CancellationTokenSource _listenerCancelTokenSource;

        public async Task StartListeningAsync(int delayMiliseconds, int timeoutMiliseconds)
        {
            _listenerCancelTokenSource = new CancellationTokenSource();
            IsListening = true;

            while (true)
            {
                Location location = await GetGpsLocationAsync(timeoutMiliseconds);
                if (location != null)
                {
                    OnRaiseGpsLocationEvent(new GpsLocationEventArgs(location, delayMiliseconds));
                }
                await TaskAddition.Delay(delayMiliseconds, _listenerCancelTokenSource.Token);

                if (_listenerCancelTokenSource.IsCancellationRequested) break;
            }

            IsListening = false;
            _listenerCancelTokenSource.Dispose();
        }

        public async Task StopListening()
        {
            if (_listenerCancelTokenSource != null && !_listenerCancelTokenSource.IsCancellationRequested)
            {
                _listenerCancelTokenSource.Cancel();
            }

            await TaskAddition.DelayUntil(() => IsListening == false, 100);
        }

        public async Task<Location> GetGpsLocationAsync(int millisecondsTimeout)
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromMilliseconds(millisecondsTimeout));
            var location = await Geolocation.Default.GetLocationAsync(request);

            return location;
        }

        private void OnRaiseGpsLocationEvent(GpsLocationEventArgs e)
        {
            var raiseEvent = RaiseGpsLocationEvent;
            raiseEvent?.Invoke(this, e);
        }

        public async Task<Location> GetLastGpsLocationAsync()
        {
            return await Geolocation.Default.GetLastKnownLocationAsync();
        }

        public async Task<bool> IsGpsEnabled(int timeoutSeconds)
        {
            try
            {
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Lowest, TimeSpan.FromSeconds(timeoutSeconds));
                _ = await Geolocation.Default.GetLocationAsync(request);
            }
            catch (FeatureNotEnabledException)
            {
                return false;
            }

            return true;
        }
    }
}
