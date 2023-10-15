using Kris.Client.Common;

namespace Kris.Client.Data
{
    public class UsersLocationIntervalDataSource : IDataSource<UsersLocationIntervalItem>
    {
        public IEnumerable<UsersLocationIntervalItem> Get()
        {
            return new List<UsersLocationIntervalItem>()
            {
                new UsersLocationIntervalItem() { Display = I18n.Keys.UsersLocationInterval2sTitle, Value = 2000 },
                new UsersLocationIntervalItem() { Display = I18n.Keys.UsersLocationInterval5sTitle, Value = 5000 },
                new UsersLocationIntervalItem() { Display = I18n.Keys.UsersLocationInterval10sTitle, Value = 10000 },
                new UsersLocationIntervalItem() { Display = I18n.Keys.UsersLocationInterval30sTitle, Value = 30000 },
                new UsersLocationIntervalItem() { Display = I18n.Keys.UsersLocationInterval1mTitle, Value = 100000 },
                new UsersLocationIntervalItem() { Display = I18n.Keys.UsersLocationInterval2mTitle, Value = 200000 },
                new UsersLocationIntervalItem() { Display = I18n.Keys.UsersLocationInterval5mTitle, Value = 500000 },
                new UsersLocationIntervalItem() { Display = I18n.Keys.UsersLocationInterval10mTitle, Value = 1000000 },
            };
        }
    }
}
