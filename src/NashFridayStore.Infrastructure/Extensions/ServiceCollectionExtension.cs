using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.Infrastructure.AppOptions;
using NashFridayStore.Infrastructure.Interfaces;
using NashFridayStore.Infrastructure.Services;
using Google.Cloud.Storage.V1;
using StackExchange.Redis;
using Microsoft.CodeAnalysis.Options;
using Stripe;

namespace NashFridayStore.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Settings at appsettings.json
        services.AddOptions<ConnectionStringsOptions>()
            .Bind(configuration.GetSection(ConnectionStringsOptions.ConnectionStrings))
            .ValidateOnStart();

        services.AddOptions<SiteUrlsOption>()
            .Bind(configuration.GetSection(SiteUrlsOption.SiteUrls))
            .ValidateOnStart();

        services.AddOptions<FirebaseOptions>()
            .Bind(configuration.GetSection(FirebaseOptions.Firebase))
            .ValidateOnStart();

        services.AddOptions<StripeOptions>()
            .Bind(configuration.GetSection(FirebaseOptions.Firebase))
            .ValidateOnStart();

        // DbContext + SQL Server + Seeder
        services.AddDbContext<StoreDbContext>((sp, options) =>
        {
            ConnectionStringsOptions settings = sp.GetRequiredService<IOptions<ConnectionStringsOptions>>().Value;
            options.UseSqlServer(settings.Database);
            options.UseSnakeCaseNamingConvention();
        });
        services.AddTransient<StoreDbContextSeeder>();

        // Firebase
        // - StorageClient is recommend as a singleton, not as a service
        services.AddScoped<IStorageService, FirebaseStorageService>();
        services.AddSingleton<StorageClient>(_ => StorageClient.Create());

        // Redis + Cart
        // - Connection Multiplexer must be singleton, designed as thread-safe and global reuse
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            ConnectionStringsOptions settings = sp.GetRequiredService<IOptions<ConnectionStringsOptions>>().Value;
            return ConnectionMultiplexer.Connect(settings.Caching);
        });
        services.AddScoped<ICartService, RedisCartService>();

        // Stripe
        services.AddSingleton(sp =>
        {
            StripeOptions settings = sp.GetRequiredService<IOptions<StripeOptions>>().Value;
            return new StripeClient(settings.SecretKey);
        });
    }
}
