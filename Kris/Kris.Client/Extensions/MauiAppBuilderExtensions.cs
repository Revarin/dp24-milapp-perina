using Kris.Client.Core;
using Kris.Client.Data;
using Kris.Client.ViewModels;

using MauiMap = Microsoft.Maui.Controls.Maps.Map;

namespace Kris.Client
{
    public static class MauiAppBuilderExtensions
    {
        public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<MapView>();
            builder.Services.AddSingleton<MenuView>();
            builder.Services.AddTransient<ConnectionSettingsView>();
            builder.Services.AddSingleton<TestView>();

            return builder;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<AppShellViewModel>();
            builder.Services.AddSingleton<MapViewModel>();
            builder.Services.AddSingleton<MenuViewModel>();
            builder.Services.AddTransient<ConnectionSettingsViewModel>();
            builder.Services.AddSingleton<TestViewModel>();

            return builder;
        }

        public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IPreferencesStore, PreferencesStore>();
            builder.Services.AddSingleton<IAlertService, AlertService>();
            builder.Services.AddSingleton<IPermissionsService, PermissionsService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IGpsService, GpsService>();

            return builder;
        }

        public static MauiAppBuilder RegisterDataSources(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<IDataSource<GpsIntervalItem>, GpsIntervalDataSource>();

            return builder;
        }

        public static MauiAppBuilder ConfigureHandlers(this MauiAppBuilder builder)
        {
            builder.ConfigureMauiHandlers(handlers =>
            {
#if ANDROID || IOS || MACCATALYST
                handlers.AddHandler<MauiMap, CustomMapHandler>();
#endif
            });

            return builder;
        }
    }
}
