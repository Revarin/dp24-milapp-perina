using Kris.Common.Enums;

namespace Kris.Client.Core.Models;

public sealed class ConversationListModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ConversationType ConversationType { get; set; }
    public int MessageCount { get; set; }
    public DateTime? LastMessage { get; set; }
}
