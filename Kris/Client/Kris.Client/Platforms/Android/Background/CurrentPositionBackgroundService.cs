using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;
using Kris.Client.Core.Background;
using Kris.Client.Utility;

namespace Kris.Client.Platforms.Background;

[Service]
public sealed class CurrentPositionBackgroundService : Service
{
    private readonly IBinder _binder = new LocalBinder();
    private readonly ICurrentPositionBackgroundHandler _handler;

    private int _id = new object().GetHashCode();
    private Timer _timer;

    public CurrentPositionBackgroundService()
    {
        _handler = ServiceHelper.GetService<ICurrentPositionBackgroundHandler>();
    }

    public override IBinder OnBind(Intent intent)
    {
        return _binder;
    }

    public static void StartService()
    {
        var intent = new Intent(MainActivity.Instance, typeof(CurrentPositionBackgroundService));
        MainActivity.Instance.StartService(intent);
    }

    public static void StopService()
    {
        var intent = new Intent(MainActivity.Instance, typeof(CurrentPositionBackgroundService));
        MainActivity.Instance.StopService(intent);
    }

    // Source: https://github.com/carlfranklin/MAUIAndroidFS/tree/master
    [return: GeneratedEnum]
    public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
    {
        var notificationIntent = new Intent(this, typeof(MainActivity));
        var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, PendingIntentFlags.Immutable);

        var notification = new NotificationCompat.Builder(this, MainApplication.KrisChannelId)
            .SetContentText("Current positions GPS")
            .SetSmallIcon(Resource.Drawable.maui_splash)
            .SetContentIntent(pendingIntent);

        StartForeground(_id, notification.Build());

        _timer = new Timer(Execute, null, TimeSpan.Zero, _handler.Interval);

        return StartCommandResult.NotSticky;
    }

    // Source: https://stackoverflow.com/a/61270701
    public override void OnTaskRemoved(Intent rootIntent)
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
        {
            StopForeground(StopForegroundFlags.Remove);
        }
        else
        {
            StopForeground(true);
        }

        _timer.Dispose();
        _timer = null;
        StopSelf();

        base.OnDestroy();
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }

    private async void Execute(object state)
    {
        await _handler.ExecuteAsync(CancellationToken.None);
        _timer.Change(_handler.Interval, _handler.Interval);
    }
}
