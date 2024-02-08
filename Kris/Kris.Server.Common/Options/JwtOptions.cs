namespace Kris.Server.Common.Options;

public sealed class JwtOptions : IOptions
{
    public static string Section => "Jwt";

    public required string Key { get; set; }
    public required int ExpirationMinutes { get; set; }
    public string? Issuer { get; set; }
}
