using Kris.Common;

namespace Kris.Server.Common.Options;

public sealed class JwtOptions : IOptions
{
    public static string Section => "Jwt";

    public required string Key { get; init; }
    public required int ExpirationMinutes { get; init; }
    public string? Issuer { get; init; }
}
