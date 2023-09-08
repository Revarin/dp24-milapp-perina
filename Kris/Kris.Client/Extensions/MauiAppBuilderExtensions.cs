using Kris.Client.Behaviors;
using Kris.Client.Common;
using Kris.Client.ViewModels;

namespace Kris.Client
{
    public static class MauiAppBuilderExtensions
    {
        public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<MapView>();
            builder.Services.AddSingleton<TestView>();

            return builder;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<MapViewModel>();
            builder.Services.AddSingleton<TestViewModel>();

            return builder;
        }

        public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<CurrentRegionBehavior>();
            builder.Services.AddSingleton<IPreferencesStore, PreferencesStore>();

            return builder;
        }
    }
}
