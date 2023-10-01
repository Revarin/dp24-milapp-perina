using Microsoft.AspNetCore.Mvc;

namespace Kris.Interface
{
    public class LocationClient : ClientBase, ILocationController
    {
        public LocationClient() : base()
        {
        }

        public ActionResult<LoadUsersLocationsResponse> LoadUsersLocations(LoadUsersLocationsRequest request)
        {
            throw new NotImplementedException();
        }

        public ActionResult SaveUserLocation(SaveUserLocationRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
