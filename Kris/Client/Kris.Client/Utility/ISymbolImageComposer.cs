using Kris.Common.Enums;

namespace Kris.Client.Utility;

public interface ISymbolImageComposer
{
    Stream ComposeMapPointSymbol(MapPointSymbolShape? pointShape, MapPointSymbolColor? pointColor, MapPointSymbolSign? pointSign);
}
