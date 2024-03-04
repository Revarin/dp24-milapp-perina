using Microsoft.Maui.Maps.Handlers;

namespace Kris.Client.Components.Map;

public partial class KrisMapHandler : MapHandler, IKrisMapHandler
{
    public static readonly IPropertyMapper<IKrisMap, IKrisMapHandler> KrisPropertyMapper = new PropertyMapper<IKrisMap, IKrisMapHandler>(Mapper)
    {
        [nameof(IKrisMap.Pins)] = MapPins,
    };
    public static readonly CommandMapper<IKrisMap, IKrisMapHandler> KrisCommandMapper = new CommandMapper<IKrisMap, IKrisMapHandler>(CommandMapper)
    {
    };

    public KrisMapHandler() : base(KrisPropertyMapper, KrisCommandMapper)
    {
    }

    public KrisMapHandler(IPropertyMapper? mapper = null, CommandMapper? commandMapper = null)
    : base(mapper ?? KrisPropertyMapper, commandMapper ?? KrisCommandMapper)
    {
    }

    public new IKrisMap VirtualView
    {
        get { return (IKrisMap)base.VirtualView; }
    }
}
