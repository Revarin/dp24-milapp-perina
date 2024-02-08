namespace Kris.Interface
{
    public interface ILocationController
    {
        Task SaveUserLocation(SaveUserLocationRequest request);
        Task<LoadUsersLocationsResponse> LoadUsersLocations(LoadUsersLocationsRequest request);
    }
}
