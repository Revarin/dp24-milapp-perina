﻿namespace Kris.Client.Behaviors;

// Source: https://github.com/xamarin/xamarin-forms-samples/blob/main/Behaviors/EventToCommandBehavior/EventToCommandBehavior/Behaviors/BehaviorBase.cs#L6
public abstract class BindableBehavior<T> : Behavior<T> where T : BindableObject
{
    public T AssociatedObject { get; private set; }

    protected override void OnAttachedTo(T bindable)
    {
        base.OnAttachedTo(bindable);
        AssociatedObject = bindable;

        if (bindable.BindingContext != null)
        {
            BindingContext = bindable.BindingContext;
        }

        bindable.BindingContextChanged += OnBindingContextChanged;
    }

    protected override void OnDetachingFrom(T bindable)
    {
        base.OnDetachingFrom(bindable);
        bindable.BindingContextChanged -= OnBindingContextChanged;
        AssociatedObject = null;
    }

    private void OnBindingContextChanged(object sender, EventArgs e)
    {
        OnBindingContextChanged();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        BindingContext = AssociatedObject.BindingContext;
    }
}
