using Kris.Common.Enums;

namespace Kris.Interface.Models;

public sealed class ConversationListModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ConversationType ConversationType { get; set; }
    public List<UserModel> Users { get; set; } = new List<UserModel>();
    public int MessageCount { get; set; }
    public DateTime? LastMessage { get; set; }
}
