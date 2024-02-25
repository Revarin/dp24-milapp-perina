using CommunityToolkit.Mvvm.Messaging;
using Kris.Client.Core.Messages;

namespace Kris.Client.Core.Services;

public interface IMessageService
{
    void Register<TMessage>(object recipient, MessageHandler<object, TMessage> handler) where TMessage : MessageBase;
    void Unregister<TMessage>(object recipient) where TMessage : MessageBase;
    void Send<TMessage>(TMessage message) where TMessage : MessageBase;
}
