using CommunityToolkit.Maui;
using Kris.Client.Core.Services;
using Kris.Client.ViewModels;
using Kris.Client.Views;
using Microsoft.Extensions.Logging;

namespace Kris.Client
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<LoginView>();
            builder.Services.AddSingleton<RegisterView>();

            builder.Services.AddSingleton<AppShellViewModel>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<RegisterViewModel>();

            builder.Services.AddSingleton<IRouterService, RouterService>();
            builder.Services.AddSingleton<IAlertService, AlertService>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
