using Microsoft.EntityFrameworkCore;
using Kris.Server.Data;
using Kris.Server.Common.Options;
using Kris.Server.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Kris.Server.Data.Repositories;
using Kris.Server.Core.Services;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Handlers;

namespace Kris.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddOptions<SettingsOptions>()
            .Bind(builder.Configuration.GetRequiredSection(SettingsOptions.Section))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        builder.Services.AddOptions<JwtOptions>()
            .Bind(builder.Configuration.GetRequiredSection(JwtOptions.Section))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining<BaseHandler>();
        });

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ISessionRepository, SessionRepository>();
        builder.Services.AddScoped<ISessionUserRepository, SessionUserRepository>();
        builder.Services.AddScoped<IUserPositionRepository, UserPositionRepository>();
        builder.Services.AddScoped<IUserSettingsRepository, UserSettingsRepository>();
        builder.Services.AddScoped<IMapPointRepository, MapPointRepository>();

        builder.Services.AddSingleton<IJwtService, JwtService>();
        builder.Services.AddSingleton<IPasswordService, PasswordService>();
        builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();

        builder.Services.AddSingleton<IUserMapper, UserMapper>();
        builder.Services.AddSingleton<ISessionMapper, SessionMapper>();
        builder.Services.AddSingleton<IPositionMapper, PositionMapper>();
        builder.Services.AddSingleton<IMapObjectMapper, MapObjectMapper>();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var jwtOptions = builder.Configuration.GetSection(JwtOptions.Section).Get<JwtOptions>();
            if (jwtOptions == null) throw new Exception(nameof(jwtOptions));
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Key))
            };
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseMiddleware<ResponseWrapperMiddleware>();
        app.UseAuthentication();
        app.UseMiddleware<ApiKeyMiddleware>();
        app.UseRouting();
        app.UseAuthorization();

        app.MapControllers();

        if (app.Environment.IsProduction())
        {
            app.Services.GetRequiredService<DataContext>().Database.Migrate();
        }

        app.Run();
    }
}
