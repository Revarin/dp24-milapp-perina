using Microsoft.AspNetCore.Mvc;
using Kris.Interface;

namespace Kris.Server
{
    public class LocationController : ControllerBase, ILocationController
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public ActionResult SaveUserLocation(SaveUserLocationRequest request)
        {
            if (request == null) return BadRequest();

            var result = _locationService.SaveUserLocation(request.UserId, request.TimeStamp, request.Location);

            if (!result) return BadRequest(_locationService.GetErrorMessage());

            return Ok();
        }

        public ActionResult<LoadUsersLocationsResponse> LoadUsersLocations(LoadUsersLocationsRequest request)
        {
            if (request == null) return BadRequest();

            var locations = _locationService.LoadUsersLocationLocations(request.UserId, request.LastUpdate);

            if (locations == null) return BadRequest(_locationService.GetErrorMessage());

            return Ok(new LoadUsersLocationsResponse
            {
                TimeStamp = DateTime.UtcNow,
                UserLocations = locations
            });
        }
    }
}
