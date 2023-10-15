using Kris.Client.Common;

namespace Kris.Client.Core
{
    public class GpsService : IGpsService
    {
        public event EventHandler<GpsLocationEventArgs> RaiseGpsLocationEvent;

        public bool IsListening { get; set; }

        private CancellationTokenSource _listenerCancelTokenSource;

        public void StartListening(int delayMiliseconds, int timeoutMiliseconds)
        {
            Task.Run(async () =>
            {
                // Wait until previous listener ended
                await TaskAddition.DelayUntil(() => _listenerCancelTokenSource == null, 50);

                _listenerCancelTokenSource = new CancellationTokenSource();
                IsListening = true;

                while (!_listenerCancelTokenSource.IsCancellationRequested)
                {
                    Location location = await GetGpsLocationAsync(timeoutMiliseconds);

                    if (location != null)
                    {
                        OnRaiseGpsLocationEvent(new GpsLocationEventArgs(location, delayMiliseconds));
                    }

                    await TaskAddition.Delay(delayMiliseconds, _listenerCancelTokenSource.Token);
                }

                IsListening = false;
                _listenerCancelTokenSource.Dispose();
                _listenerCancelTokenSource = null;
            });
        }

        public void StopListening()
        {
            if (_listenerCancelTokenSource != null && !_listenerCancelTokenSource.IsCancellationRequested)
            {
                _listenerCancelTokenSource.Cancel();
            }
        }

        public async Task<Location> GetGpsLocationAsync(int millisecondsTimeout)
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromMilliseconds(millisecondsTimeout));
            var location = await Geolocation.Default.GetLocationAsync(request);

            return location;
        }

        public async Task<Location> GetLastGpsLocationAsync()
        {
            return await Geolocation.Default.GetLastKnownLocationAsync();
        }

        public async Task<bool> IsGpsEnabledAsync(int timeoutSeconds)
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

        private void OnRaiseGpsLocationEvent(GpsLocationEventArgs e)
        {
            var raiseEvent = RaiseGpsLocationEvent;
            Application.Current.Dispatcher.Dispatch(() => raiseEvent?.Invoke(this, e));
        }
    }
}
