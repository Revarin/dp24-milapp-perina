﻿using CommunityToolkit.Maui;
using Kris.Client.Common.Options;
using Kris.Client.Connection;
using Kris.Client.Connection.Clients;
using Kris.Client.Core.Handlers;
using Kris.Client.Core.Services;
using Kris.Client.ViewModels;
using Kris.Client.Views;
using Kris.Interface.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

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

            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("Kris.Client.appsettings.json");
            var config = new ConfigurationBuilder().AddJsonStream(stream).Build();

            builder.Configuration.AddConfiguration(config);
            builder.Services.AddOptions();
            builder.Services.Configure<SettingsOptions>(builder.Configuration.GetRequiredSection(SettingsOptions.Section));

            builder.Services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblyContaining(typeof(BaseHandler));
            });

            builder.Services.AddTransient<LoginView>();
            builder.Services.AddTransient<RegisterView>();

            builder.Services.AddSingleton<AppShellViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();

            builder.Services.AddSingleton<IRouterService, RouterService>();
            builder.Services.AddSingleton<IAlertService, AlertService>();

            builder.Services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
            builder.Services.AddTransient<IUserController, UserClient>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}