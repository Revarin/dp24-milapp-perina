using Kris.Interface.Models;

namespace Kris.Client.Connection.Hubs.Events;

public sealed class MessageReceivedEventArgs : EventArgs
{
    public Guid Id { get; init; }
    public Guid? SenderId { get; init; }
    public string? SenderName { get; init; }
    public string Body { get; init; }
    public DateTime TimeStamp { get; init; }

    public MessageReceivedEventArgs(MessageModel model)
    {
        Id = model.Id;
        SenderId = model.SenderId;
        SenderName = model.SenderName;
        Body = model.Body;
        TimeStamp = model.TimeStamp;
    }
}
