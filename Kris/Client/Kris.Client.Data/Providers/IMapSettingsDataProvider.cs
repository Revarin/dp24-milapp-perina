using Kris.Client.Data.Models;
using Kris.Client.Data.Models.Picker;

namespace Kris.Client.Data.Providers;

public interface IMapSettingsDataProvider : ISettingsDataProvider<MapSettingsEntity>
{
    IEnumerable<CoordinateSystemItem> GetCoordinateSystemItems();
    CoordinateSystemItem GetCurrentCoordinateSystem();
}
