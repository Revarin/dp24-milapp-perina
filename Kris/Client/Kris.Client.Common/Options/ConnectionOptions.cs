using Kris.Common;

namespace Kris.Client.Common.Options;

public sealed class ConnectionOptions : IOptions
{
    public static string Section => "Connection";

    public string ApiUrl { get; init; }
    public string ApiKey { get; init; }
    public int HubKeepAliveSeconds { get; init; }
    public int HubServerTimeoutSeconds { get; init; }
}
