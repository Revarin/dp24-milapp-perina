using Kris.Client.Common.Options;
using Kris.Client.Data.Cache;
using Kris.Client.Data.Models;
using Kris.Client.Data.Models.Picker;
using Kris.Client.Data.Static;
using Kris.Client.Data.Static.Picker;
using Microsoft.Extensions.Options;

namespace Kris.Client.Data.Providers;

public sealed class ConnectionSettingsDataProvider : IConnectionSettingsDataProvider
{
    private readonly ISettingsStore _settingsStore;
    private readonly DefaultPreferencesOptions _defaultOptions;
    private readonly IStaticDataSource<GpsIntervalSettingsItem> _gpsIntervalSettingsItems;
    private readonly IStaticDataSource<PositionDownloadSettingsItem> _positionDownloadSettingsItems;
    private readonly IStaticDataSource<PositionUploadSettingsItem> _positionUploadSettingsItems;
    private readonly IStaticDataSource<MapObjectDownloadSettingsItem> _mapObjectDownloadSettingsItems;

    public ConnectionSettingsDataProvider(ISettingsStore settingsStore, IOptions<DefaultPreferencesOptions> options)
    {
        _settingsStore = settingsStore;
        _defaultOptions = options.Value;
        _gpsIntervalSettingsItems = new GpsIntervalSettingsDataSource();
        _positionDownloadSettingsItems = new PositionDownloadSettingsDataSource();
        _positionUploadSettingsItems = new PositionUploadSettingsDataSource();
        _mapObjectDownloadSettingsItems = new MapObjectDownloadSettingsDataSource();
    }

    public GpsIntervalSettingsItem GetCurrentGpsIntervalSettings()
    {
        var settings = _settingsStore.GetConnectionSettings();
        if (settings?.GpsInterval != null)
        {
            var currentValue = _gpsIntervalSettingsItems.Get().FirstOrDefault(i => i.Value.Equals(settings.GpsInterval));
            if (currentValue != null) return currentValue;
        }

        return _gpsIntervalSettingsItems.Get().First(i => IsEqualSeconds(i.Value, _defaultOptions.GpsInterval));
    }

    public MapObjectDownloadSettingsItem GetCurrentMapObjectDownloadSettings()
    {
        var settings = _settingsStore.GetConnectionSettings();
        if (settings?.MapObjectDownloadInterval != null)
        {
            var currentValue = _mapObjectDownloadSettingsItems.Get().FirstOrDefault(i => i.Value.Equals(settings.MapObjectDownloadInterval));
            if (currentValue != null) return currentValue;
        }

        return _mapObjectDownloadSettingsItems.Get().First(i => IsEqualSeconds(i.Value, _defaultOptions.MapObjectDownloadFrequency));
    }

    public PositionDownloadSettingsItem GetCurrentPositionDownloadSettings()
    {
        var settings = _settingsStore.GetConnectionSettings();
        if (settings?.PositionDownloadInterval != null)
        {
            var currentValue = _positionDownloadSettingsItems.Get().FirstOrDefault(i => i.Value.Equals(settings.PositionDownloadInterval));
            if (currentValue != null) return currentValue;
        }

        return _positionDownloadSettingsItems.Get().First(i => IsEqualSeconds(i.Value, _defaultOptions.PositionDownloadFrequency));
    }

    public PositionUploadSettingsItem GetCurrentPositionUploadSettings()
    {
        var settings = _settingsStore.GetConnectionSettings();
        if (settings?.PositionUploadMultiplier != null)
        {
            var currentValue = _positionUploadSettingsItems.Get().FirstOrDefault(i => i.Value == settings.PositionUploadMultiplier);
            if (currentValue != null) return currentValue;
        }

        return _positionUploadSettingsItems.Get().First(i => i.Value == _defaultOptions.PositionUploadFrequency);
    }

    public ConnectionSettingsEntity GetDefault()
    {
        return new ConnectionSettingsEntity
        {
            GpsInterval = _gpsIntervalSettingsItems.Get().First(i => IsEqualSeconds(i.Value, _defaultOptions.GpsInterval)).Value,
            PositionDownloadInterval = _positionDownloadSettingsItems.Get().First(i => IsEqualSeconds(i.Value, _defaultOptions.PositionDownloadFrequency)).Value,
            PositionUploadMultiplier = _positionUploadSettingsItems.Get().First(i => i.Value == _defaultOptions.PositionUploadFrequency).Value,
            MapObjectDownloadInterval = _mapObjectDownloadSettingsItems.Get().First(i => IsEqualSeconds(i.Value, _defaultOptions.MapObjectDownloadFrequency)).Value
        };
    }

    public IEnumerable<GpsIntervalSettingsItem> GetGpsIntervalSettingsItems()
    {
        return _gpsIntervalSettingsItems.Get();
    }

    public IEnumerable<MapObjectDownloadSettingsItem> GetMapObjectDownloadSettingsItems()
    {
        return _mapObjectDownloadSettingsItems.Get();
    }

    public IEnumerable<PositionDownloadSettingsItem> GetPositionDownloadSettingsItems()
    {
        return _positionDownloadSettingsItems.Get();
    }

    public IEnumerable<PositionUploadSettingsItem> GetPositionUploadSettingsItems()
    {
        return _positionUploadSettingsItems.Get();
    }

    private bool IsEqualSeconds(TimeSpan first, int second) => first.CompareTo(TimeSpan.FromSeconds(second)) == 0;
}
