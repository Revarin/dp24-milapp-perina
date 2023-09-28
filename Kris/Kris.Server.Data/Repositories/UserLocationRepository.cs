namespace Kris.Server.Data
{
    public class UserLocationRepository : RepositoryBase<UserLocationEntity>, IUserLocationRepository
    {
        public UserLocationRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public IQueryable<UserLocationViewModel> GetUserLocations(int userId)
        {
            var db = _context;
            var query = from location in db.Locations
                        where location.UserId != userId
                        join user in db.Users on location.UserId equals user.Id
                        select new UserLocationViewModel
                        {
                            Id = location.Id,
                            UserId = location.UserId,
                            UserName = user.Name,
                            Latitude = location.Latitude,
                            Longitude = location.Longitude,
                            UpdatedDate = location.UpdatedDate
                        };

            return query;
        }

        public IQueryable<UserLocationViewModel> GetUserLocations(int userId, DateTime dateFrom)
        {
            var db = _context;
            var query = from location in db.Locations
                        where location.UserId != userId && location.UpdatedDate > dateFrom
                        join user in db.Users on location.UserId equals user.Id
                        select new UserLocationViewModel
                        {
                            Id = location.Id,
                            UserId = location.UserId,
                            UserName = user.Name,
                            Latitude = location.Latitude,
                            Longitude = location.Longitude,
                            UpdatedDate = location.UpdatedDate
                        };

            return query;
        }
    }
}
