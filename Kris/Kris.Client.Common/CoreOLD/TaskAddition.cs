namespace Kris.Client.Common
{
    public static class TaskAddition
    {
        // Source: https://stackoverflow.com/a/32768637
        public static Task<bool> Delay(int millisecondsDelay, CancellationToken token)
        {
            return Task.Delay(millisecondsDelay, token).ContinueWith(tsk => tsk.Exception == default);
        }

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
}
