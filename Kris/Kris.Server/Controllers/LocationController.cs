using Microsoft.AspNetCore.Mvc;
using Kris.Interface;

namespace Kris.Server
{
    [Route("[controller]/[action]")]
    public class LocationController : ControllerBase, ILocationController
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpPost]
        public Task SaveUserLocation(SaveUserLocationRequest request)
        {
            if (request == null) throw new BadHttpRequestException("Missing request body");

            var result = _locationService.SaveUserLocation(request.UserId, request.TimeStamp, request.Location);

            if (!result) throw new BadHttpRequestException(_locationService.GetErrorMessage());

            return Task.CompletedTask;
        }

        [HttpPost]
        public Task<LoadUsersLocationsResponse> LoadUsersLocations(LoadUsersLocationsRequest request)
        {
            if (request == null) throw new BadHttpRequestException("Missing request body");

            var locations = _locationService.LoadUsersLocationLocations(request.UserId, request.LastUpdate);

            if (locations == null) throw new BadHttpRequestException(_locationService.GetErrorMessage());

            return Task.FromResult(new LoadUsersLocationsResponse
            {
                TimeStamp = DateTime.UtcNow,
                UserLocations = locations
            });
        }
    }
}
