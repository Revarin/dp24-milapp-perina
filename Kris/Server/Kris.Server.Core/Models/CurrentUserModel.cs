using Kris.Common.Enums;

namespace Kris.Server.Core.Models;

public sealed class CurrentUserModel
{
    public required Guid UserId { get; init; }
    public required string Login { get; init; }
    public Guid? SessionId { get; set; }
    public UserType? UserType { get; set; }
    public string? SessionName { get; set; }
}
