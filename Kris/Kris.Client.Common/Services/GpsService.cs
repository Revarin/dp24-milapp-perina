namespace Kris.Client.Common
{
    public class GpsService : IGpsService
    {
        public event EventHandler<GpsLocationEventArgs> RaiseGpsLocationEvent;
        
        public bool IsListening { get; set; }

        private CancellationTokenSource _listenerCancelTokenSource;
        private CancellationTokenSource _requestCancelTokenSource;
        private bool _isCheckingLocation;

        public async Task StartListeningAsync(int millisecondsDelay, int millisecondsTimeout)
        {
            _listenerCancelTokenSource = new CancellationTokenSource();
            IsListening = true;

            Location location = null;
            while (true)
            {
                location = await GetGpsLocationAsync(millisecondsTimeout);
                if (location != null)
                {
                    OnRaiseGpsLocationEvent(new GpsLocationEventArgs(location));
                }
                await Task.Delay(millisecondsDelay);
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
    }
}
