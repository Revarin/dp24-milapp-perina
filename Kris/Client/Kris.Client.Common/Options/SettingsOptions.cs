using Kris.Common;

namespace Kris.Client.Common.Options;

public sealed class SettingsOptions : IOptions
{
    public static string Section => "AppSettings";

    public int LoginExpirationMinutes { get; set; }
    public int ChatMessagesPageSize { get; set; }
}
