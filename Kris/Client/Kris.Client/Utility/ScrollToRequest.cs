using Kris.Client.Behaviors.Events;

namespace Kris.Client.Utility;

public sealed class ScrollToRequest : IViewRequest<int>
{
    public event EventHandler<ScrollToEventArgs> ScrollToRequested;

    public void Execute(int data)
    {
        ScrollToRequested?.Invoke(this, new ScrollToEventArgs(data));
    }
}
