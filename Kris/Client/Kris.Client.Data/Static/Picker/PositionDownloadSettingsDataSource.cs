using Kris.Client.Data.Models.Picker;

namespace Kris.Client.Data.Static.Picker;

public sealed class PositionDownloadSettingsDataSource : IStaticDataSource<PositionDownloadSettingsItem>
{
    private List<PositionDownloadSettingsItem> _source = new List<PositionDownloadSettingsItem>
    {
        new PositionDownloadSettingsItem { Value = TimeSpan.FromSeconds(5), Display = "5 seconds" },
        new PositionDownloadSettingsItem { Value = TimeSpan.FromSeconds(10), Display = "10 seconds" },
        new PositionDownloadSettingsItem { Value = TimeSpan.FromSeconds(30), Display = "30 seconds" },
        new PositionDownloadSettingsItem { Value = TimeSpan.FromSeconds(60), Display = "60 seconds" },
        new PositionDownloadSettingsItem { Value = TimeSpan.FromSeconds(120), Display = "120 seconds" },
    };
    
    public IEnumerable<PositionDownloadSettingsItem> Get()
    {
        return _source;
    }
}
