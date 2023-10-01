using Microsoft.AspNetCore.Mvc;

namespace Kris.Interface
{
    public interface ILocationController
    {
        ActionResult SaveUserLocation(SaveUserLocationRequest request);
        ActionResult<LoadUsersLocationsResponse> LoadUsersLocations(LoadUsersLocationsRequest request);
    }
}
