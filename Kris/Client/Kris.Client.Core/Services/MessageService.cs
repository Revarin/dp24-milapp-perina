using CommunityToolkit.Mvvm.Messaging;
using Kris.Client.Core.Messages;

namespace Kris.Client.Core.Services;

public sealed class MessageService : IMessageService
{
    public void Register<TMessage>(object recipient, MessageHandler<object, TMessage> handler) where TMessage : MessageBase
    {
        WeakReferenceMessenger.Default.Register(recipient, handler);
    }

    public void Send<TMessage>(TMessage message) where TMessage : MessageBase
    {
        WeakReferenceMessenger.Default.Send(message);
    }

    public void Unregister<TMessage>(object recipient) where TMessage : MessageBase
    {
        WeakReferenceMessenger.Default.Unregister<TMessage>(recipient);
    }
}
