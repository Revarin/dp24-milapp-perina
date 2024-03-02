using Microsoft.Maui.Maps.Handlers;

namespace Kris.Client.Components.Map;

public interface IKrisMapHandler : IMapHandler
{
    new IKrisMap VirtualView { get; }
}
