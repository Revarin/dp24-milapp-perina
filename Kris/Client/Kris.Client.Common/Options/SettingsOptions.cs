using Kris.Common;

namespace Kris.Client.Common.Options;

public sealed class SettingsOptions : IOptions
{
    public static string Section => "AppSettings";

    public int LoginExpirationMinutes { get; init; }
    public int ChatMessagesPageSize { get; init; }
}
