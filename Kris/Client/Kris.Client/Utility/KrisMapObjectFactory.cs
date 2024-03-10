using Kris.Client.Common.Enums;
using Kris.Client.Components.Map;
using Kris.Client.Core.Models;
using Kris.Client.Data.Cache;
using Kris.Client.ViewModels.Views;

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
        var pin = new KrisMapPinViewModel
        {
            Id = userId,
            CreatorId = userId,
            Name = userName,
            TimeStamp = DateTime.Now,
            Location = location,
            KrisPinType = KrisPinType.Self,
            ImageName = "point_green.png",
            View = () => new KrisPin(userName, "point_green.png", false)
        };
        return pin;
    }

    public KrisMapPinViewModel CreateUserPositionPin(UserPositionModel userPosition)
    {
        var pin = new KrisMapPinViewModel
        {
            Id = userPosition.UserId,
            CreatorId = userPosition.UserId,
            Name = userPosition.UserName,
            TimeStamp = userPosition.Updated,
            Location = userPosition.Positions.First(),
            KrisPinType = KrisPinType.User,
            ImageName = "point_blue.png",
            View = () => new KrisPin(userPosition.UserName, "point_blue.png", false)
        };
        return pin;
    }

    public KrisMapPinViewModel CreateMapPoint(MapPointModel mapPoint)
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
            CreatorId = mapPoint.Creator.Id,
            Name = mapPoint.Name,
            TimeStamp = mapPoint.Created,
            Location = mapPoint.Location,
            Description = mapPoint.Description,
            KrisPinType = KrisPinType.Point,
            ImageName = symbolName,
            View = () => new KrisPin(mapPoint.Name, symbolName, true)
        };
        return pin;
    }
}
