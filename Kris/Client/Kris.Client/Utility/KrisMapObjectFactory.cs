using Kris.Client.Common.Enums;
using Kris.Client.Core.Models;
using Kris.Client.Data.Cache;
using Kris.Client.ViewModels.Items;

namespace Kris.Client.Utility;

public sealed class KrisMapObjectFactory : IKrisMapObjectFactory
{
    private readonly ISymbolImageComposer _symbolImageComposer;
    private readonly IFileStore _symbolImageCache;

    public KrisMapObjectFactory(ISymbolImageComposer symbolImageComposer, IFileStore symbolImageCache)
    {
        _symbolImageComposer = symbolImageComposer;
        _symbolImageCache = symbolImageCache;
    }

    public KrisMapPinViewModel CreateUserPositionPin(UserPositionModel userPosition, KrisPinType krisPinType)
    {
        var symbol = userPosition.Symbol;
        var symbolName = $"point_{symbol.Shape}_{symbol.Color}_{symbol.Sign}.png";

        if (!_symbolImageCache.CacheExists(symbolName))
        {
            var imageStream = _symbolImageComposer.ComposeMapPointSymbol(symbol.Shape, symbol.Color, symbol.Sign);
            _symbolImageCache.SaveToCache(symbolName, imageStream);
        }

        var pin = new KrisMapPinViewModel
        {
            Id = userPosition.UserId,
            Name = userPosition.UserName,
            CreatorId = userPosition.UserId,
            CreatorName = userPosition.UserName,
            TimeStamp = userPosition.Updated,
            Location = userPosition.Positions.First(),
            KrisPinType = krisPinType,
            ImageName = symbolName
        };
        return pin;
    }

    public KrisMapPinViewModel CreateMapPoint(MapPointListModel mapPoint)
    {
        var symbol = mapPoint.Symbol;
        var symbolName = $"point_{symbol.Shape}_{symbol.Color}_{symbol.Sign}.png";

        if (!_symbolImageCache.CacheExists(symbolName))
        {
            var imageStream = _symbolImageComposer.ComposeMapPointSymbol(symbol.Shape, symbol.Color, symbol.Sign);
            _symbolImageCache.SaveToCache(symbolName, imageStream);
        }

        var pin = new KrisMapPinViewModel
        {
            Id = mapPoint.Id,
            Name = mapPoint.Name,
            CreatorId = mapPoint.Creator.Id,
            CreatorName = mapPoint.Creator.Name,
            TimeStamp = mapPoint.Created,
            Location = mapPoint.Location,
            KrisPinType = KrisPinType.Point,
            ImageName = symbolName
        };
        return pin;
    }
}
