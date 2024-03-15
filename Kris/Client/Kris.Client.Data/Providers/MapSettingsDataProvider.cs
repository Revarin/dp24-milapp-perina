using Kris.Client.Common.Options;
using Kris.Client.Data.Cache;
using Kris.Client.Data.Models;
using Kris.Client.Data.Models.Picker;
using Kris.Client.Data.Static;
using Kris.Client.Data.Static.Picker;
using Microsoft.Extensions.Options;

namespace Kris.Client.Data.Providers;

public sealed class MapSettingsDataProvider : IMapSettingsDataProvider
{
    private readonly ISettingsStore _settingsStore;
    private readonly DefaultPreferencesOptions _defaultOptions;
    private readonly IStaticDataSource<CoordinateSystemItem> _coordinateSystemItems;

    public MapSettingsDataProvider(ISettingsStore settingsStore, IOptions<DefaultPreferencesOptions> defaultOptions)
    {
        _settingsStore = settingsStore;
        _defaultOptions = defaultOptions.Value;

        _coordinateSystemItems = new CoordinateSystemDataSource();
    }

    public IEnumerable<CoordinateSystemItem> GetCoordinateSystemItems()
    {
        return _coordinateSystemItems.Get();
    }

    public CoordinateSystemItem GetCurrentCoordinateSystem()
    {
        var settings = _settingsStore.GetMapSettings();
        if (settings?.CoordinateSystem != null)
        {
            var currentValue = _coordinateSystemItems.Get().FirstOrDefault(i => i.Value == settings.CoordinateSystem.Value);
            if (currentValue != null) return currentValue;
        }

        return _coordinateSystemItems.Get().First(i => i.Value == _defaultOptions.CoordinateSystem);
    }

    public MapSettingsEntity GetDefault()
    {
        return new MapSettingsEntity
        {
            CoordinateSystem = _coordinateSystemItems.Get().First(i => i.Value == _defaultOptions.CoordinateSystem).Value
        };
    }
}
