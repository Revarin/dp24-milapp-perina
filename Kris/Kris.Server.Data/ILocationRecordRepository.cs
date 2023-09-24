namespace Kris.Server.Data
{
    public interface ILocationRecordRepository : IRepository<LocationRecordEntity>
    {
        IQueryable<LocationRecordEntity> GetUpdates(DateTime from);
    }
}
