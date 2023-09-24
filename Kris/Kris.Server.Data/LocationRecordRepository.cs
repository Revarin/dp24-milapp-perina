namespace Kris.Server.Data
{
    public class LocationRecordRepository : RepositoryBase<LocationRecordEntity>, ILocationRecordRepository
    {
        public LocationRecordRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public IQueryable<LocationRecordEntity> GetUpdates(DateTime dateFrom)
        {
            var db = _context;
            var query = from location in db.LocationRecords
                        where location.UpdatedDate > dateFrom
                        select location;

            return query;
        }
    }
}
