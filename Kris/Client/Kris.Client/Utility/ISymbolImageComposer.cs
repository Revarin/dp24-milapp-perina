using Kris.Common.Enums;

namespace Kris.Client.Utility;

public interface ISymbolImageComposer
{
    ImageSource ComposeMapPointSymbol(MapPointSymbolShape? pointShape, MapPointSymbolColor? pointColor, MapPointSymbolSign? pointSign);
}
