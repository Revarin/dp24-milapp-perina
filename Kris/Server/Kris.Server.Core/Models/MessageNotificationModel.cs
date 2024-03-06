using Kris.Interface.Models;

namespace Kris.Server.Core.Models;

public sealed class MessageNotificationModel
{
    public required List<Guid> UsersToNotify { get; set; } = new List<Guid>();
    public required MessageModel Message { get; set; }
}
