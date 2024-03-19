using SQLite;

namespace Kris.Client.Data.Models.Database;

[Table("info")]
public sealed class InfoEntity
{
    public int MinZoom { get; set; }
    public int MaxZoom { get; set; }
    public double Center_X { get; set; }
    public double Center_Y { get; set; }
    public string Zooms { get; set; }
    public int Provider { get; set; }
}
