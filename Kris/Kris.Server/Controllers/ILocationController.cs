using Kris.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Kris.Server
{
    public interface ILocationController
    {
        ActionResult SaveUserLocation(SaveUserLocationRequest request);
        ActionResult<LoadUsersLocationsResponse> LoadUsersLocations(LoadUsersLocationsRequest request);
    }
}
