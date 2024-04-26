namespace Kris.Client.Common.Metrics;

public static class SentryMetrics
{
    private const bool Disabled = true;

    public static void CounterIncrement(string key)
    {
        if (Disabled) return;
        var tags = new Dictionary<string, string> { { "app", "kris" } };
        SentrySdk.Metrics.Increment(key, tags: tags, timestamp: DateTimeOffset.UtcNow);
    }

    public static IDisposable TimerStart(string key)
    {
        if (Disabled) return null;
        var tags = new Dictionary<string, string> { { "app", "kris" } };
        return SentrySdk.Metrics.StartTimer(key, tags: tags);
    }

    public static void SetAdd(string key, int value)
    {
        if (Disabled) return;
        var tags = new Dictionary<string, string> { { "app", "kris" } };
        SentrySdk.Metrics.Set(key, value, tags: tags, timestamp: DateTimeOffset.UtcNow);
    }
}
