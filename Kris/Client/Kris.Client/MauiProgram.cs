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
using Kris.Client.Components.Map;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Kris.Client.Utility;
using Kris.Client.Connection.Hubs;
using CoordinateSharp;
using Kris.Client.Core.Image;
using Kris.Client.Data.Database;
using Kris.Client.Components.Utility;

namespace Kris.Client
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.UseMauiApp<App>()
                .UseSkiaSharp()
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
                options.AddHandler<KrisMap, KrisMapHandler>();
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
            builder.Services.AddTransient<MapSettingsView>();
            builder.Services.AddTransient<MapSettingsViewModel>();
            builder.Services.AddTransient<ContactsView>();
            builder.Services.AddTransient<ContactsViewModel>();
            builder.Services.AddTransient<ChatView>();
            builder.Services.AddTransient<ChatViewModel>();

            builder.Services.AddTransientPopup<PasswordPopup, PasswordPopupViewModel>();
            builder.Services.AddTransientPopup<CreateSessionPopup, CreateSessionPopupViewModel>();
            builder.Services.AddTransientPopup<EditSessionPopup, EditSessionPopupViewModel>();
            builder.Services.AddTransientPopup<CreateMapPointPopup, CreateMapPointPopupViewModel>();
            builder.Services.AddTransientPopup<EditMapPointPopup, EditMapPointPopupViewModel>();
            builder.Services.AddTransientPopup<ImagePopup, ImagePopupViewModel>();
            builder.Services.AddTransientPopup<LoadingPopup, LoadingPopupViewModel>();

            builder.Services.AddSingleton<ISymbolImageComposer, SymbolImageComposer>();
            builder.Services.AddSingleton<IKrisMapObjectFactory, KrisMapObjectFactory>();
            builder.Services.AddTransient<IImageAttachmentComposer, ImageAttachmentComposer>();

            builder.Services.AddSingleton<IUserMapper, UserMapper>();
            builder.Services.AddSingleton<ISessionMapper, SessionMapper>();
            builder.Services.AddSingleton<IPositionMapper, PositionMapper>();
            builder.Services.AddSingleton<ISettingsMapper, SettingsMapper>();
            builder.Services.AddSingleton<IMapObjectsMapper, MapObjectsMapper>();
            builder.Services.AddSingleton<IConversationMapper, ConversationMapper>();
            builder.Services.AddSingleton<IMessageMapper, MessageMapper>();

            builder.Services.AddSingleton<IBackgroundLoop, BackgroundLoop>();
            builder.Services.AddSingleton<ICurrentPositionBackgroundHandler, CurrentPositionBackgroundHandler>();
            builder.Services.AddSingleton<IUserPositionsBackgroundHandler, UserPositionsBackgroundHandler>();
            builder.Services.AddSingleton<IMapObjectsBackgroundHandler, MapObjectsBackgroundHandler>();

            builder.Services.AddSingleton<IRouterService, RouterService>();
            builder.Services.AddSingleton<IAlertService, AlertService>();
            builder.Services.AddSingleton<IGpsService, GpsService>();
            builder.Services.AddSingleton<IPermissionService, PermissionService>();
            builder.Services.AddSingleton<IMessageService, MessageService>();
            builder.Services.AddSingleton<IClipboardService, ClipboardService>();
            builder.Services.AddSingleton<IMediaService, MediaService>();

            builder.Services.AddSingleton<IIdentityStore, IdentityStore>();
            builder.Services.AddSingleton<ILocationStore, LocationStore>();
            builder.Services.AddSingleton<ISettingsStore, SettingsStore>();
            builder.Services.AddSingleton<IFileStore, FileStore>();

            builder.Services.AddSingleton<IRepositoryFactory, RepositoryFactory>();

            builder.Services.AddTransient<IConnectionSettingsDataProvider, ConnectionSettingsDataProvider>();
            builder.Services.AddTransient<IMapPointSymbolDataProvider, MapPointSymbolDataProvider>();
            builder.Services.AddTransient<IMapSettingsDataProvider,  MapSettingsDataProvider>();

            builder.Services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
            builder.Services.AddTransient<IUserController, UserClient>();
            builder.Services.AddTransient<ISessionController, SessionClient>();
            builder.Services.AddTransient<IPositionController, PositionClient>();
            builder.Services.AddTransient<IMapObjectController, MapObjectClient>();
            builder.Services.AddTransient<IConversationController, ConversationClient>();

            builder.Services.AddSingleton<MessageClient>();
            builder.Services.AddSingleton<IMessageHub>(s => s.GetRequiredService<MessageClient>());
            builder.Services.AddSingleton<Connection.Hubs.IMessageReceiver>(s => s.GetRequiredService<MessageClient>());

#if DEBUG
            builder.Logging.AddDebug();
#endif

            GlobalSettings.Default_EagerLoad = new EagerLoad(EagerLoadType.UTM_MGRS);

            return builder.Build();
        }
    }
}
