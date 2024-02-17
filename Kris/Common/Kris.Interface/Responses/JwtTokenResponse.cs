namespace Kris.Interface.Responses;

public sealed class JwtTokenResponse : EmptyResponse
{
    public required string Token { get; set; }
}
