namespace Kris.Interface.Models;

public sealed class MessageModel
{
    public Guid Id { get; set; }
    public Guid? SenderId { get; set; }
    public string? SenderName { get; set; }
    public string Body { get; set; }
    public DateTime TimeStamp { get; set; }
}
