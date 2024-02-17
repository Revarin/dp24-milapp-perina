using Kris.Common;

namespace Kris.Client.Common.Options;

public sealed class SettingsOptions : IOptions
{
    public static string Section => "AppSettings";

    public string ApiUrl { get; set; }
    public string ApiKey { get; set; }
}
