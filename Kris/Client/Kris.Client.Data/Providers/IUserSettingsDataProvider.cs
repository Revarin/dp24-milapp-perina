using Kris.Client.Data.Models.Picker;

namespace Kris.Client.Data.Providers;

public interface IUserSettingsDataProvider
{
    IEnumerable<GpsIntervalSettingsItem> GetGpsIntervalSettingsItems();
    IEnumerable<PositionUploadSettingsItem> GetPositionUploadSettingsItems();
    IEnumerable<PositionDownloadSettingsItem> GetPositionDownloadSettingsItems();
    GpsIntervalSettingsItem GetDefaultGpsIntervalSettings();
    PositionUploadSettingsItem GetDefaultPositionUploadSettings();
    PositionDownloadSettingsItem GetDefaultPositionDownloadSettings();
}
