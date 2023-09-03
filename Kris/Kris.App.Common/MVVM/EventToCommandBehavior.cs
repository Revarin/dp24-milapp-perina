using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace Kris.App.Common
{
    // Source: https://github.com/xamarin/xamarin-forms-samples/blob/main/Behaviors/EventToCommandBehavior/EventToCommandBehavior/Behaviors/EventToCommandBehavior.cs
    public class EventToCommandBehavior : BehaviorBase<VisualElement>
    {
        private Delegate EventHandler;

        public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string), typeof(EventToCommandBehavior),
            null, propertyChanged: OnEventNameChanged);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(EventToCommandBehavior),
            null);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(EventToCommandBehavior),
            null);
        public static readonly BindableProperty InputConverterProperty = BindableProperty.Create("Converter", typeof(IValueConverter), typeof(EventToCommandBehavior),
            null);

        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(InputConverterProperty); }
            set { SetValue(InputConverterProperty, value); }
        }

        protected override void OnAttachedTo(VisualElement bindable)
        {
            base.OnAttachedTo(bindable);
            RegisterEvent(EventName);
        }

        protected override void OnDetachingFrom(VisualElement bindable)
        {
            DeregisterEvent(EventName);
            base.OnDetachingFrom(bindable);
        }

        private void RegisterEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName)) return;

            var eventInfo = AssociatedObject.GetType().GetRuntimeEvent(eventName);
            if (eventInfo == null)
            {
                throw new ArgumentException(string.Format("EventToCommandBehavior: Can't register the '{0}' event.", EventName));
            }

            var methodInfo = typeof(EventToCommandBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
            EventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
            eventInfo.AddEventHandler(AssociatedObject, EventHandler);
        }

        private void DeregisterEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName)) return;
            if (EventHandler == null) return;

            var eventInfo = AssociatedObject.GetType().GetRuntimeEvent(eventName);
            if (eventInfo == null)
            {
                throw new ArgumentException(string.Format("EventToCommandBehavior: Can't de-register the '{0}' event.", EventName));
            }

            eventInfo.RemoveEventHandler(AssociatedObject, EventHandler);
            EventHandler = null;
        }

        private void OnEvent(object sender, EventArgs e)
        {
            if (Command == null) return;

            object resolvedParameter;
            if (CommandParameter != null)
            {
                resolvedParameter = CommandParameter;
            }
            else if (Converter != null)
            {
                resolvedParameter = Converter.Convert(e, typeof(object), null, null);
            }
            else
            {
                resolvedParameter = e;
            }

            if (Command.CanExecute(resolvedParameter))
            {
                Command.Execute(resolvedParameter);
            }
        }

        private static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (EventToCommandBehavior)bindable;
            if (behavior.AssociatedObject == null) return;

            string oldEventName = (string)oldValue;
            string newEventName = (string)newValue;

            behavior.DeregisterEvent(oldEventName);
            behavior.RegisterEvent(newEventName);
        }
    }
}
