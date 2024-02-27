using Kris.Client.Data.Models.Picker;

namespace Kris.Client.Data.Static.Picker;

public sealed class GpsIntervalSettingsDataSource : IStaticDataSource<GpsIntervalSettingsItem>
{
    private List<GpsIntervalSettingsItem> _source = new List<GpsIntervalSettingsItem>
    {
        new GpsIntervalSettingsItem { Value = TimeSpan.FromSeconds(2), Display = "2 seconds" },
        new GpsIntervalSettingsItem { Value = TimeSpan.FromSeconds(5), Display = "5 seconds" },
        new GpsIntervalSettingsItem { Value = TimeSpan.FromSeconds(10), Display = "10 seconds" },
        new GpsIntervalSettingsItem { Value = TimeSpan.FromSeconds(30), Display = "30 seconds" },
        new GpsIntervalSettingsItem { Value = TimeSpan.FromSeconds(60), Display = "60 seconds" },
    };

    public IEnumerable<GpsIntervalSettingsItem> Get()
    {
        return _source;
    }
}
