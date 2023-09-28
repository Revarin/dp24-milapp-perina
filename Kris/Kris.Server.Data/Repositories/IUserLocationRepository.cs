namespace Kris.Server.Data
{
    public interface IUserLocationRepository : IRepository<UserLocationEntity>
    {
        IQueryable<UserLocationViewModel> GetUserLocations(int userId);
        IQueryable<UserLocationViewModel> GetUserLocations(int userId, DateTime from);
    }
}
