using Kris.Client.Common.Enums;
using Kris.Client.Core.Models;
using Kris.Client.Data.Cache;
using Kris.Client.ViewModels.Views;
using Kris.Common.Enums;

namespace Kris.Client.Utility;

public sealed class KrisMapObjectFactory : IKrisMapObjectFactory
{
    private readonly ISymbolImageComposer _symbolImageComposer;
    private readonly ISymbolImageCache _symbolImageCache;

    public KrisMapObjectFactory(ISymbolImageComposer symbolImageComposer, ISymbolImageCache symbolImageCache)
    {
        _symbolImageComposer = symbolImageComposer;
        _symbolImageCache = symbolImageCache;
    }

    public KrisMapPinViewModel CreateMyPositionPin(Guid userId, string userName, Location location)
    {
        var symbolName = $"point_{MapPointSymbolShape.Circle}_{MapPointSymbolColor.Green}_{MapPointSymbolSign.None}.png";

        if (!_symbolImageCache.Exists(symbolName))
        {
            var imageStream = _symbolImageComposer.ComposeMapPointSymbol(MapPointSymbolShape.Circle, MapPointSymbolColor.Green, MapPointSymbolSign.None);
            _symbolImageCache.Save(symbolName, imageStream);
        }

        var pin = new KrisMapPinViewModel
        {
            Id = userId,
            Name = userName,
            CreatorId = userId,
            CreatorName = userName,
            TimeStamp = DateTime.Now,
            Location = location,
            KrisPinType = KrisPinType.Self,
            ImageName = symbolName
        };
        return pin;
    }

    public KrisMapPinViewModel CreateUserPositionPin(UserPositionModel userPosition)
    {
        var symbolName = $"point_{MapPointSymbolShape.Circle}_{MapPointSymbolColor.Blue}_{MapPointSymbolSign.None}.png";

        if (!_symbolImageCache.Exists(symbolName))
        {
            var imageStream = _symbolImageComposer.ComposeMapPointSymbol(MapPointSymbolShape.Circle, MapPointSymbolColor.Blue, MapPointSymbolSign.None);
            _symbolImageCache.Save(symbolName, imageStream);
        }

        var pin = new KrisMapPinViewModel
        {
            Id = userPosition.UserId,
            Name = userPosition.UserName,
            CreatorId = userPosition.UserId,
            CreatorName = userPosition.UserName,
            TimeStamp = userPosition.Updated,
            Location = userPosition.Positions.First(),
            KrisPinType = KrisPinType.User,
            ImageName = symbolName
        };
        return pin;
    }

    public KrisMapPinViewModel CreateMapPoint(MapPointListModel mapPoint)
    {
        var symbol = mapPoint.Symbol;
        var symbolName = $"point_{symbol.Shape}_{symbol.Color}_{symbol.Sign}.png";

        if (!_symbolImageCache.Exists(symbolName))
        {
            var imageStream = _symbolImageComposer.ComposeMapPointSymbol(symbol.Shape, symbol.Color, symbol.Sign);
            _symbolImageCache.Save(symbolName, imageStream);
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
