namespace Kris.Interface.Responses;

public sealed class JwtTokenResponse : ResponseBase
{
    public required string Token { get; set; }
}
