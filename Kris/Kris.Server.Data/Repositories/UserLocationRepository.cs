using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data
{
    public class UserLocationRepository : RepositoryBase<UserLocationEntity>, IUserLocationRepository
    {
        public UserLocationRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public UserLocationEntity GetUserLocation(int userId)
        {
            var db = _context;
            var query = from location in db.Locations
                        where location.UserId == userId
                        select location;

            return query.AsNoTracking().FirstOrDefault();
        }

        public IQueryable<UserLocationViewModel> GetUsersLocations(int excludedUserId)
        {
            var db = _context;
            var query = from location in db.Locations
                        where location.UserId != excludedUserId
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

            return query.AsNoTracking();
        }

        public IQueryable<UserLocationViewModel> GetUsersLocations(int excludedUserId, DateTime dateFrom)
        {
            var db = _context;
            var query = from location in db.Locations
                        where location.UserId != excludedUserId && location.UpdatedDate > dateFrom
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

            return query.AsNoTracking();
        }
    }
}
