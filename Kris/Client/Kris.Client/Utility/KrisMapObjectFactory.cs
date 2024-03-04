using Kris.Client.Common.Enums;
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
            ImageSource = ImageSource.FromFile("point_green.png"),
            ImageName = "point_green.png"
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
            ImageSource = ImageSource.FromFile("point_blue.png"),
            ImageName = "point_blue.png"
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
            ImageSource = _symbolImageCache.Load(symbolName),
            ImageName = symbolName
        };
        return pin;
    }
}
