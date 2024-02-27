using CommunityToolkit.Maui;
using Kris.Client.Common.Options;
using Kris.Client.Components.Popups;
using Kris.Client.Connection;
using Kris.Client.Connection.Clients;
using Kris.Client.Core.Handlers;
using Kris.Client.Core.Listeners;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Services;
using Kris.Client.Data.Cache;
using Kris.Client.ViewModels.Popups;
using Kris.Client.ViewModels.Views;
using Kris.Client.Views;
using Kris.Interface.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Kris.Client.Data.Providers;
using Kris.Client.Platforms.Map;

using MauiMap = Microsoft.Maui.Controls.Maps.Map;

namespace Kris.Client
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.UseMauiApp<App>()
                .UseMauiMaps()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.ConfigureMauiHandlers(options =>
            {
#if ANDROID || IOS
                options.AddHandler<MauiMap, CustomMapHandler>();
#endif
            });

            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("Kris.Client.appsettings.json");
            var config = new ConfigurationBuilder().AddJsonStream(stream).Build();

            builder.Configuration.AddConfiguration(config);
            builder.Services.AddOptions();
            builder.Services.Configure<ConnectionOptions>(builder.Configuration.GetRequiredSection(ConnectionOptions.Section));
            builder.Services.Configure<SettingsOptions>(builder.Configuration.GetRequiredSection(SettingsOptions.Section));
            builder.Services.Configure<DefaultPreferencesOptions>(builder.Configuration.GetRequiredSection(DefaultPreferencesOptions.Section));

            builder.Services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblyContaining(typeof(BaseHandler));
            });

            builder.Services.AddSingleton<AppShellViewModel>();

            builder.Services.AddTransient<LoginView>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegisterView>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddSingleton<MapView>();
            builder.Services.AddSingleton<MapViewModel>();
            builder.Services.AddSingleton<MenuView>();
            builder.Services.AddSingleton<MenuViewModel>();
            builder.Services.AddTransient<SessionSettingsView>();
            builder.Services.AddTransient<SessionSettingsViewModel>();
            builder.Services.AddTransient<UserSettingsView>();
            builder.Services.AddTransient<UserSettingsViewModel>();

            builder.Services.AddTransientPopup<PasswordPopup, PasswordPopupViewModel>();
            builder.Services.AddTransientPopup<EditSessionPopup, EditSessionPopupViewModel>();

            builder.Services.AddSingleton<IUserMapper, UserMapper>();
            builder.Services.AddSingleton<ISessionMapper, SessionMapper>();
            builder.Services.AddSingleton<IPositionMapper, PositionMapper>();
            builder.Services.AddSingleton<ISettingsMapper, SettingsMapper>();

            builder.Services.AddSingleton<ICurrentPositionListener, CurrentPositionListener>();
            builder.Services.AddSingleton<IUserPositionsListener, UserPositionsListener>();

            builder.Services.AddSingleton<IRouterService, RouterService>();
            builder.Services.AddSingleton<IAlertService, AlertService>();
            builder.Services.AddSingleton<IGpsService, GpsService>();
            builder.Services.AddSingleton<IPermissionService, PermissionService>();
            builder.Services.AddSingleton<IMessageService, MessageService>();

            builder.Services.AddSingleton<IIdentityStore, IdentityStore>();
            builder.Services.AddSingleton<ILocationStore, LocationStore>();
            builder.Services.AddSingleton<ISettingsStore, SettingsStore>();

            builder.Services.AddTransient<IConnectionSettingsDataProvider, ConnectionSettingsDataProvider>();

            builder.Services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
            builder.Services.AddTransient<IUserController, UserClient>();
            builder.Services.AddTransient<ISessionController, SessionClient>();
            builder.Services.AddTransient<IPositionController, PositionClient>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
