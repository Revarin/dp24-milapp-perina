using Kris.Common.Enums;

namespace Kris.Server.Data.Models;

public class MessageEntity : EntityBase
{
    public string? Body { get; set; }
    public required MessageType MessageType { get; set; }
    public required DateTime TimeStamp { get; set; }
    public required Guid ConversationId { get; set; }
    public ConversationEntity? Conversation { get; set; }
    public Guid? SenderId { get; set; }
    public SessionUserEntity? Sender { get; set; }
}
