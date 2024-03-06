namespace Kris.Interface.Requests;

public sealed class SendMessageRequest : RequestBase
{
    public Guid ConversationId { get; set; }
    public string Message { get; set; }
    public DateTime Sent { get; set; }
}
