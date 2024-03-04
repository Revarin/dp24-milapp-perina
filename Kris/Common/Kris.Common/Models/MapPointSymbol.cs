using Kris.Common.Enums;

namespace Kris.Common.Models;

public record MapPointSymbol
{
    public MapPointSymbolShape Shape { get; set; }
    public MapPointSymbolColor Color { get; set; }
    public MapPointSymbolSign Sign { get; set; }
}
