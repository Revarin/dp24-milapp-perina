using Kris.Common;

namespace Kris.Server.Common.Options;

public sealed class SettingsOptions : IOptions
{
    public static string Section => "AppSettings";

    public required string ApiKey { get; init; }
    public required int SignalRKeepAliveSeconds { get; init; }
}
