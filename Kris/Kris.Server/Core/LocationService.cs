﻿using Kris.Interface;
using Kris.Server.Data;

namespace Kris.Server
{
    public class LocationService : ServiceBase, ILocationService
    {
        private readonly IUserLocationRepository _locationRepo;
        private readonly IUserRepository _userRepo;

        public LocationService(IUserLocationRepository locationRepo, IUserRepository userRepo)
        {
            _locationRepo = locationRepo;
            _userRepo = userRepo;
        }

        public bool SaveUserLocation(int userId, DateTime timeStamp, UserLocation location)
        {
            if (!_userRepo.UserExists(userId)) return SetErrorMessage("User does not exists", false);

            var userLocation = _locationRepo.GetUserLocation(userId);

            if (userLocation != null)
            {
                _locationRepo.Update(new UserLocationEntity
                {
                    Id = userLocation.Id,
                    UserId = userId,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    UpdatedDate = timeStamp
                });
            }
            else
            {
                _locationRepo.Insert(new UserLocationEntity
                {
                    UserId = userId,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    UpdatedDate = timeStamp
                });
            }
            
            return true;
        }

        public IEnumerable<UserLocation> LoadUsersLocationLocations(int userId, DateTime? timeStamp)
        {
            if (!_userRepo.UserExists(userId)) return SetErrorMessage<IEnumerable<UserLocation>>("User does not exists", null);

            var locations = timeStamp.HasValue ?
                _locationRepo.GetUsersLocations(userId, timeStamp.Value) :
                _locationRepo.GetUsersLocations(userId);

            return locations.Select(s => new UserLocation
            {
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                UserId = s.UserId,
                UserName = s.UserName
            }).ToList();
        }
    }
}
