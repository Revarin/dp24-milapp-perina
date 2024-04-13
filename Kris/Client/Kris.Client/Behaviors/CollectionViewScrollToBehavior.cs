using Kris.Client.Behaviors.Events;
using ScrollToRequest = Kris.Client.Utility.ScrollToRequest;

namespace Kris.Client.Behaviors;

public sealed class CollectionViewScrollToBehavior : BindableBehavior<CollectionView>
{
    public static readonly BindableProperty RequestProperty = BindableProperty.Create(
        "ScrollRequest", typeof(ScrollToRequest), typeof(CollectionViewScrollToBehavior), default(ScrollToRequest), BindingMode.OneWay,
        propertyChanged: OnScrollToChanged);

    public ScrollToRequest Request
    {
        get { return (ScrollToRequest)GetValue(RequestProperty); }
        set { SetValue(RequestProperty, value); }
    }

    private static void OnScrollToChanged(BindableObject bindable, object oldValue, object newValue)
    {
        (bindable as CollectionViewScrollToBehavior)?.OnScrollToChanged(oldValue as ScrollToRequest, newValue as ScrollToRequest);
    }

    private void OnScrollToChanged(ScrollToRequest oldValue, ScrollToRequest newValue)
    {
        if (oldValue != null)
        {
            oldValue.ScrollToRequested -= OnScrollToRequested;
        }
        if (newValue != null)
        {
            newValue.ScrollToRequested += OnScrollToRequested;
        }
    }

    private void OnScrollToRequested(object sender, ScrollToEventArgs e)
    {
        AssociatedObject.ScrollTo(e.Index, animate: false);
    }
}
