using Kris.Common.Enums;

namespace Kris.Interface.Responses;

public sealed class LoginResponse : Response
{
    public Guid UserId { get; set; }
    public string Login { get; set; }
    public Guid? SessionId { get; set; }
    public string? SessionName { get; set; }
    public UserType? UserType { get; set; }
    public string Token { get; set; }
}
