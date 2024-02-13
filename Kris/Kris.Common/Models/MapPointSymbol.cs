using Kris.Common.Enums;

namespace Kris.Common.Models;

public record MapPointSymbol
{
    public required MapPointSymbolShape Shape { get; set; }
    public required MapPointSymbolColor Color { get; set; }
    public required MapPointSymbolSign Sign { get; set; }
}
