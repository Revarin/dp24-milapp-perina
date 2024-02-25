using Kris.Common;

namespace Kris.Client.Common.Options;

public sealed class ConnectionOptions : IOptions
{
    public static string Section => "Connection";

    public string ApiUrl { get; set; }
    public string ApiKey { get; set; }
}
