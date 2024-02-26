using Kris.Client.Common.Options;
using Kris.Client.Data.Models.Picker;
using Kris.Client.Data.Static;
using Kris.Client.Data.Static.Picker;
using Microsoft.Extensions.Options;

namespace Kris.Client.Data.Providers;

public sealed class UserSettingsDataProvider : IUserSettingsDataProvider
{
    private readonly DefaultPreferencesOptions _defaultOptions;
    private readonly IStaticDataSource<GpsIntervalSettingsItem> _gpsIntervalSettingsItems;
    private readonly IStaticDataSource<PositionDownloadSettingsItem> _positionDownloadSettingsItems;
    private readonly IStaticDataSource<PositionUploadSettingsItem> _positionUploadSettingsItems;

    public UserSettingsDataProvider(IOptions<DefaultPreferencesOptions> options)
    {
        _defaultOptions = options.Value;
        _gpsIntervalSettingsItems = new GpsIntervalSettingsDataSource();
        _positionDownloadSettingsItems = new PositionDownloadSettingsDataSource();
        _positionUploadSettingsItems = new PositionUploadSettingsDataSource();
    }

    public GpsIntervalSettingsItem GetDefaultGpsIntervalSettings()
    {
        return _gpsIntervalSettingsItems.Get().FirstOrDefault(i => i.Value == _defaultOptions.GpsInterval);
    }

    public PositionDownloadSettingsItem GetDefaultPositionDownloadSettings()
    {
        return _positionDownloadSettingsItems.Get().FirstOrDefault(i => i.Value == _defaultOptions.PositionDownloadFrequency);
    }

    public PositionUploadSettingsItem GetDefaultPositionUploadSettings()
    {
        return _positionUploadSettingsItems.Get().FirstOrDefault(i => i.Value == _defaultOptions.PositionUploadFrequency);
    }

    public IEnumerable<GpsIntervalSettingsItem> GetGpsIntervalSettingsItems()
    {
        return _gpsIntervalSettingsItems.Get();
    }

    public IEnumerable<PositionDownloadSettingsItem> GetPositionDownloadSettingsItems()
    {
        return _positionDownloadSettingsItems.Get();
    }

    public IEnumerable<PositionUploadSettingsItem> GetPositionUploadSettingsItems()
    {
        return _positionUploadSettingsItems.Get();
    }
}
