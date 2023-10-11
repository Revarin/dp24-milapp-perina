namespace Kris.Server.Data
{
    public interface IUserLocationRepository : IRepository<UserLocationEntity>
    {
        UserLocationEntity GetUserLocation(int userId);
        IQueryable<UserLocationViewModel> GetUsersLocations(int excludedUserId);
        IQueryable<UserLocationViewModel> GetUsersLocations(int excludedUserId, DateTime from);
    }
}
