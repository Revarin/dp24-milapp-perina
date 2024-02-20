using Kris.Common.Enums;

namespace Kris.Interface.Responses;

public sealed class LoginResponse : Response
{
    public required Guid UserId { get; set; }
    public required string Login { get; set; }
    public Guid? SessionId { get; set; }
    public string? SessionName { get; set; }
    public UserType? UserType { get; set; }
    public required string Token { get; set; }
}
