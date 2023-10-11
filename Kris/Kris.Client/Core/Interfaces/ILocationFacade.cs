using Kris.Client.Core;

namespace Kris.Client
{
    public interface ILocationFacade
    {
        event EventHandler<UsersLocationsEventArgs> RaiseUserLocationsEvent;
        DateTime? LastUpdate { get; set; }
        bool IsListening { get; }

        Task SaveUserLocationAsync(int userId, Location location);
        Task StartListeningToUserLocationsAsync(int userId, int delayMiliseconds);
        Task StopListening();
    }
}
