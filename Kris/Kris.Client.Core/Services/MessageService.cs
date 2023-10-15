using CommunityToolkit.Mvvm.Messaging;

namespace Kris.Client.Core
{
    public class MessageService : IMessageService
    {
        public void Register<TMessage>(object recipient, MessageHandler<object, TMessage> handler) where TMessage : MessageBase
        {
            WeakReferenceMessenger.Default.Register(recipient, handler);
        }

        public void Unregister<TMessage>(object recipient) where TMessage : MessageBase
        {
            WeakReferenceMessenger.Default.Unregister<TMessage>(recipient);
        }

        public void Send<TMessage>(TMessage message) where TMessage : MessageBase
        {
            WeakReferenceMessenger.Default.Send(message);
        }
    }
}
