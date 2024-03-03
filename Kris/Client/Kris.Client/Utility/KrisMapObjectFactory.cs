using Kris.Client.Common.Enums;
using Kris.Client.Components.Map;
using Kris.Client.Core.Models;
using Kris.Client.Data.Cache;

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

    public KrisMapPin CreateMyPositionPin(Guid userId, string userName, Location location)
    {
        var pin = new KrisMapPin
        {
            Id = userId,
            Name = userName,
            TimeStamp = DateTime.Now,
            Location = location,
            PinType = KrisPinType.Self,
            ImageSource = ImageSource.FromFile("point_green.png")
        };
        return pin;
    }

    public KrisMapPin CreateUserPositionPin(UserPositionModel userPosition)
    {
        var pin = new KrisMapPin
        {
            Id = userPosition.UserId,
            Name = userPosition.UserName,
            TimeStamp = userPosition.Updated,
            Location = userPosition.Positions.First(),
            PinType = KrisPinType.User,
            ImageSource = ImageSource.FromFile("point_blue.png")
        };
        return pin;
    }

    public KrisMapPin CreateMapPoint(MapPointModel mapPoint)
    {
        var symbol = mapPoint.Symbol;
        var symbolName = $"point_{symbol.Shape}_{symbol.Color}_{symbol.Sign}.png";

        if (!_symbolImageCache.Exists(symbolName))
        {
            var imageStream = _symbolImageComposer.ComposeMapPointSymbol(symbol.Shape, symbol.Color, symbol.Sign);
            _symbolImageCache.Save(symbolName, imageStream);
        }

        var pin = new KrisMapPin
        {
            Id = mapPoint.Id,
            Name = mapPoint.Name,
            TimeStamp = mapPoint.Created,
            Location = mapPoint.Location,
            Description = mapPoint.Description,
            PinType = KrisPinType.Point,
            ImageSource = _symbolImageCache.Load(symbolName)
        };
        return pin;
    }
}
