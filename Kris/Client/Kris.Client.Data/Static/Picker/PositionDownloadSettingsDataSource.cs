using Kris.Client.Data.Models.Picker;

namespace Kris.Client.Data.Static.Picker;

public sealed class PositionDownloadSettingsDataSource : IStaticDataSource<PositionDownloadSettingsItem>
{
    private List<PositionDownloadSettingsItem> _source = new List<PositionDownloadSettingsItem>
    {
        new PositionDownloadSettingsItem { Value = 5, Display = "5 seconds" },
        new PositionDownloadSettingsItem { Value = 10, Display = "10 seconds" },
        new PositionDownloadSettingsItem { Value = 30, Display = "30 seconds" },
        new PositionDownloadSettingsItem { Value = 60, Display = "60 seconds" },
        new PositionDownloadSettingsItem { Value = 120, Display = "120 seconds" },
    };
    
    public IEnumerable<PositionDownloadSettingsItem> Get()
    {
        return _source;
    }
}
