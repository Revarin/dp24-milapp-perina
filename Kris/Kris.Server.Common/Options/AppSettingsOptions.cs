namespace Kris.Server.Common.Options;

public sealed class AppSettingsOptions
{
    public const string Section = "AppSettings";

    public required string ApiKey { get; set; }
}
