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
        public static readonly string KrisChannelId = "krisBackgroundServiceChannel";

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
                var serviceChannel = new NotificationChannel(KrisChannelId, "Kris Background Service Channel", NotificationImportance.High);

                if (GetSystemService(NotificationService) is NotificationManager manager)
                {
                    manager.CreateNotificationChannel(serviceChannel);
                }
            }
        }
    }
}
