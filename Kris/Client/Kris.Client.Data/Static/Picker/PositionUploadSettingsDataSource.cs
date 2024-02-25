using Kris.Client.Data.Models.Picker;

namespace Kris.Client.Data.Static.Picker;

public sealed class PositionUploadSettingsDataSource : IStaticDataSource<PositionUploadSettingsItem>
{
    private List<PositionUploadSettingsItem> _source = new List<PositionUploadSettingsItem>
    {
        new PositionUploadSettingsItem { Value = 1, Display = "Every GPS request" },
        new PositionUploadSettingsItem { Value = 2, Display = "Once per 2 requests" },
        new PositionUploadSettingsItem { Value = 3, Display = "Once per 3 GPS requests" },
        new PositionUploadSettingsItem { Value = 5, Display = "Once every 5 GPS requests" },
        new PositionUploadSettingsItem { Value = 10, Display = "Once every 10 GPS requests" },
    };

    public IEnumerable<PositionUploadSettingsItem> Get()
    {
        return _source;
    }
}
