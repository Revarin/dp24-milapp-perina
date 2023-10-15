using CommunityToolkit.Mvvm.Messaging;

namespace Kris.Client.Core
{
    public interface IMessageService
    {
        void Register<TMessage>(object recipient, MessageHandler<object, TMessage> handler) where TMessage : MessageBase;
        void Unregister<TMessage>(object recipient) where TMessage : MessageBase;
        void Send<TMessage>(TMessage message) where TMessage : MessageBase;
    }
}
