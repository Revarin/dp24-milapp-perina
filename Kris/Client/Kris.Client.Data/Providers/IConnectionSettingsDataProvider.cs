using Kris.Client.Data.Models;
using Kris.Client.Data.Models.Picker;

namespace Kris.Client.Data.Providers;

public interface IConnectionSettingsDataProvider : ISettingsDataProvider<ConnectionSettingsEntity>
{
    IEnumerable<GpsIntervalSettingsItem> GetGpsIntervalSettingsItems();
    IEnumerable<PositionUploadSettingsItem> GetPositionUploadSettingsItems();
    IEnumerable<PositionDownloadSettingsItem> GetPositionDownloadSettingsItems();
    GpsIntervalSettingsItem GetCurrentGpsIntervalSettings();
    PositionUploadSettingsItem GetCurrentPositionUploadSettings();
    PositionDownloadSettingsItem GetCurrentPositionDownloadSettings();
}
