﻿using Kris.Client.Core;

namespace Kris.Client
{
    public interface ILocationFacade
    {
        event EventHandler<UsersLocationsEventArgs> RaiseUserLocationsEvent;
        DateTime? LastUpdate { get; set; }
        bool IsListening { get; }

        Task SaveUserLocationAsync(int userId, Location location);
        void StartListeningToUsersLocations(int userId, int delayMiliseconds);
        void StopListeningToUsersLocation();
    }
}
