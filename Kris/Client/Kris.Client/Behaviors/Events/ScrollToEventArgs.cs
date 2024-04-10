namespace Kris.Client.Behaviors.Events;

public sealed class ScrollToEventArgs : EventArgs
{
    public int Index { get; init; }

    public ScrollToEventArgs(int index)
    {
        Index = index;
    }
}
