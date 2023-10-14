using Kris.Client.Common;
using Kris.Client.Core;
using Kris.Interface;

namespace Kris.Client
{
    public class LocationFacade : ILocationFacade
    {
        private readonly ILocationController _locationClient;

        public event EventHandler<UsersLocationsEventArgs> RaiseUserLocationsEvent;

        public DateTime? LastUpdate { get; set; }
        public bool IsListening { get; private set; }

        private CancellationTokenSource _listenerCancelTokenSource;

        public LocationFacade(ILocationController locationClient)
        {
            _locationClient = locationClient;
        }

        public async Task SaveUserLocationAsync(int userId, Location location)
        {
            await _locationClient.SaveUserLocation(new SaveUserLocationRequest
            {
                Location = new UserLocation
                {
                    Latitude = location.Latitude,
                    Longitude = location.Longitude
                },
                TimeStamp = DateTime.UtcNow,
                UserId = userId
            });
        }

        public void StartListeningToUserLocations(int userId, int delayMiliseconds)
        {
            Task.Run(async () =>
            {
                if (_listenerCancelTokenSource != null)
                {
                    await TaskAddition.DelayUntil(() => _listenerCancelTokenSource == null, 50);
                }

                _listenerCancelTokenSource = new CancellationTokenSource();
                IsListening = true;

                while (!_listenerCancelTokenSource.IsCancellationRequested)
                {
                    var locationsResponse = await _locationClient.LoadUsersLocations(new LoadUsersLocationsRequest
                    {
                        UserId = userId,
                        LastUpdate = LastUpdate,
                    });

                    if (locationsResponse != null)
                    {
                        OnRaiseUserLocationsEvent(new UsersLocationsEventArgs(
                            locationsResponse.TimeStamp,
                            locationsResponse.UserLocations.Select(s => new Data.UserLocation
                            {
                                UserId = s.UserId,
                                UserName = s.UserName,
                                Location = new Location(s.Latitude, s.Longitude)
                            })));

                        LastUpdate = locationsResponse.TimeStamp;
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

        private void OnRaiseUserLocationsEvent(UsersLocationsEventArgs e)
        {
            var raiseEvent = RaiseUserLocationsEvent;
            Application.Current.Dispatcher.Dispatch(() => raiseEvent?.Invoke(this, e));
        }
    }
}
