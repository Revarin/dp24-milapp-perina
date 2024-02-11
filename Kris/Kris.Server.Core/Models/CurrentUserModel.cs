using Kris.Common.Enums;

namespace Kris.Server.Core.Models;

public sealed class CurrentUserModel
{
    public required Guid Id { get; init; }
    public required string Login { get; init; }
    public Guid? SessionId { get; init; }
    public string? SessionName { get; init; }
    public UserType? Type { get; init; }
}
