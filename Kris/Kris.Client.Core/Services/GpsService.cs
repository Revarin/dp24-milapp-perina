namespace Kris.Client.Core
{
    public class GpsService : IGpsService
    {
        public event EventHandler<GpsLocationEventArgs> RaiseGpsLocationEvent;

        public bool IsListening { get; set; }

        private CancellationTokenSource _listenerCancelTokenSource;
        private CancellationTokenSource _requestCancelTokenSource;
        private bool _isCheckingLocation;

        private int? _delay;
        private int? _timeout;

        public void SetupListener(int millisecondsDelay, int millisecondsTimeout)
        {
            _delay = millisecondsDelay;
            _timeout = millisecondsTimeout;
        }

        public async Task StartListeningAsync()
        {
            if (!_delay.HasValue || !_timeout.HasValue) throw new Exception("Call SetupListener before starting listening");

            _listenerCancelTokenSource = new CancellationTokenSource();
            IsListening = true;

            while (true)
            {
                Location location = await GetGpsLocationAsync(_timeout.Value);
                if (location != null)
                {
                    OnRaiseGpsLocationEvent(new GpsLocationEventArgs(location, _delay.Value));
                }
                await Task.Delay(_delay.Value);
            }
        }

        public void StopListening()
        {
            if (_listenerCancelTokenSource != null && !_listenerCancelTokenSource.IsCancellationRequested)
            {
                RaiseGpsLocationEvent = null;
                IsListening = false;
                _listenerCancelTokenSource.Cancel();
            }
        }

        public async Task<Location> GetGpsLocationAsync(int millisecondsTimeout)
        {
            Location location = null;

            try
            {
                _isCheckingLocation = true;

                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromMilliseconds(millisecondsTimeout));

                _requestCancelTokenSource = new CancellationTokenSource();

                location = await Geolocation.Default.GetLocationAsync(request, _requestCancelTokenSource.Token);
            }
            finally
            {
                _isCheckingLocation = false;
            }

            return location;
        }

        public void CancelRequest()
        {
            if (_isCheckingLocation && _requestCancelTokenSource != null && !_requestCancelTokenSource.IsCancellationRequested)
            {
                _requestCancelTokenSource.Cancel();
            }
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

        public async Task<bool> IsGpsEnabled()
        {
            try
            {
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Lowest, TimeSpan.FromSeconds(2));
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
