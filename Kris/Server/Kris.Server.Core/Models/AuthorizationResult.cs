using Kris.Common.Enums;

namespace Kris.Server.Core.Models;

public sealed class AuthorizationResult
{
    public Guid SessionUserId { get; set; }
    public UserType UserType { get; set; }
    public required bool IsAuthorized { get; set; }
}
