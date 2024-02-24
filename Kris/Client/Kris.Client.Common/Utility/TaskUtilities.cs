namespace Kris.Client.Common.Utility;

public static class TaskUtilities
{
    // Source: https://stackoverflow.com/a/32768637
    public static Task<bool> Delay(TimeSpan delay, CancellationToken token)
    {
        return Task.Delay(delay, token).ContinueWith(tsk => tsk.Exception == default);
    }

    public static async Task DelayUntil(Func<bool> condition, int frequency)
    {
        while (!condition())
        {
            await Task.Delay(frequency);
        }
    }

    public static async Task DelayUntil(Func<bool> condition, int frequency, CancellationToken token)
    {
        while (!condition())
        {
            await Task.Delay(frequency, token);
        }
    }
}
