using Kris.Client.Data.Models.Picker;
using Kris.Common.Enums;

namespace Kris.Client.Data.Static.Picker;

public sealed class CoordinateSystemDataSource : IStaticDataSource<CoordinateSystemItem>
{
    private List<CoordinateSystemItem> _source = new List<CoordinateSystemItem>()
    {
        new CoordinateSystemItem { Value = CoordinateSystem.LatLong, Display = "Latitude-Longitude" },
        new CoordinateSystemItem { Value = CoordinateSystem.UTM, Display = "UTM (Universal Transverse Mercator)" },
        new CoordinateSystemItem { Value = CoordinateSystem.MGRS, Display = "MGRS (Military Grid Reference System)" }
    };

    public IEnumerable<CoordinateSystemItem> Get()
    {
        return _source;
    }
}
