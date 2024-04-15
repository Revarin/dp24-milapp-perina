using Android.App;
using Android.OS;
using Android.Runtime;

namespace Kris.Client
{
#if DEBUG
    [Application(UsesCleartextTraffic = true)]
#else
    [Application]                               
#endif
    public class MainApplication : MauiApplication
    {
        public static readonly string KrisNotificationGroupId = "krisNotificationGroup";
        public static readonly string KrisBackgroundServiceChannelId = "krisBackgroundServiceChannel";
        public static readonly string KrisMessageChannelId = "krisMessageChannel";

        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        // Source: https://github.com/carlfranklin/MAUIAndroidFS/tree/master
        public override void OnCreate()
        {
            base.OnCreate();

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var group = new NotificationChannelGroup(KrisNotificationGroupId, "Kris");
                var backgroundChannel = new NotificationChannel(KrisBackgroundServiceChannelId, "Kris Background Services", NotificationImportance.Low);
                var messageChannel = new NotificationChannel(KrisMessageChannelId, "Kris Messages", NotificationImportance.Default);
                messageChannel.Group = KrisNotificationGroupId;
                backgroundChannel.Group = KrisNotificationGroupId;

                if (GetSystemService(NotificationService) is NotificationManager manager)
                {
                    manager.CreateNotificationChannelGroup(group);
                    manager.CreateNotificationChannel(backgroundChannel);
                    manager.CreateNotificationChannel(messageChannel);
                }
            }
        }
    }
}
