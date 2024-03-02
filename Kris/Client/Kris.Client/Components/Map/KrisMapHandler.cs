using Microsoft.Maui.Maps.Handlers;

namespace Kris.Client.Components.Map;

public partial class KrisMapHandler : MapHandler, IKrisMapHandler
{
    public KrisMapHandler() : base(Mapper, CommandMapper)
    {
    }

    public KrisMapHandler(IPropertyMapper? mapper = null, CommandMapper? commandMapper = null)
    : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
    {
    }

    public new IKrisMap VirtualView
    {
        get { return (IKrisMap)base.VirtualView; }
    }
}
