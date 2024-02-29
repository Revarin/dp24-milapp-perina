using Kris.Client.Data.Models.Picker;

namespace Kris.Client.Data.Static.Picker;

public sealed class MapObjectDownloadSettingsDataSource : IStaticDataSource<MapObjectDownloadSettingsItem>
{
    private List<MapObjectDownloadSettingsItem> _source = new List<MapObjectDownloadSettingsItem>
    {
        new MapObjectDownloadSettingsItem { Value = TimeSpan.FromSeconds(20), Display = "20 seconds" },
        new MapObjectDownloadSettingsItem { Value = TimeSpan.FromSeconds(60), Display = "60 seconds" },
        new MapObjectDownloadSettingsItem { Value = TimeSpan.FromSeconds(120), Display = "2 minutes" },
        new MapObjectDownloadSettingsItem { Value = TimeSpan.FromSeconds(300), Display = "5 minutes" },
        new MapObjectDownloadSettingsItem { Value = TimeSpan.FromSeconds(600), Display = "10 minutes" }
    };

    public IEnumerable<MapObjectDownloadSettingsItem> Get() => _source;
}
