using Kris.Client.Common;

namespace Kris.Client.Data
{
    public class GpsIntervalDataSource : IDataSource<GpsIntervalItem>
    {
        public IEnumerable<GpsIntervalItem> Get()
        {
            return new List<GpsIntervalItem>()
            {
                new GpsIntervalItem() { Display = I18n.Keys.GpsInterval2sTitle, Value = 2000 },
                new GpsIntervalItem() { Display = I18n.Keys.GpsInterval5sTitle, Value = 5000 },
                new GpsIntervalItem() { Display = I18n.Keys.GpsInterval10sTitle, Value = 10000 },
                new GpsIntervalItem() { Display = I18n.Keys.GpsInterval30sTitle, Value = 30000 },
                new GpsIntervalItem() { Display = I18n.Keys.GpsInterval1mTitle, Value = 100000 },
                new GpsIntervalItem() { Display = I18n.Keys.GpsInterval2mTitle, Value = 200000 },
                new GpsIntervalItem() { Display = I18n.Keys.GpsInterval5mTitle, Value = 500000 },
                new GpsIntervalItem() { Display = I18n.Keys.GpsInterval10mTitle, Value = 1000000 },
            };
        }
    }
}
