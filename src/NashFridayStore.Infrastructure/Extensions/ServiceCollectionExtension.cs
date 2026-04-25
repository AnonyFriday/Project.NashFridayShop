using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.Infrastructure.AppOptions;
using Microsoft.Extensions.Options;

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

        services.AddOptions<ClientUrlsOption>()
            .Bind(configuration.GetSection(ClientUrlsOption.ClientUrls))
            .ValidateOnStart();

        // DbContext + SQL Server + Seeder
        services.AddDbContext<StoreDbContext>((sp, options) =>
        {
            ConnectionStringsOptions settings = sp.GetRequiredService<IOptions<ConnectionStringsOptions>>().Value;
            options.UseSqlServer(settings.Database);
            options.UseSnakeCaseNamingConvention();
        });
        services.AddTransient<StoreDbContextSeeder>();
    }
}
