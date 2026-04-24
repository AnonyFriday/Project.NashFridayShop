using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NashFridayStore.IdentityServer.AppOptions;
using NashFridayStore.IdentityServer.Data;
using NashFridayStore.IdentityServer.Domain;

namespace NashFridayStore.IdentityServer.Extensions;

public static class IdentityServerCollectionExtension
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Settings at appsettings.json
        services.AddOptions<ConnectionStringsOptions>()
            .Bind(configuration.GetSection(ConnectionStringsOptions.ConnectionStrings))
            .ValidateOnStart();

        services.AddOptions<ClientUrlsOption>()
            .Bind(configuration.GetSection(ClientUrlsOption.ClientUrls))
            .ValidateOnStart();

        // DbContext + SQL Server + Seeder
        services.AddDbContext<IdentityServerDbContext>((sp, options) =>
        {
            ConnectionStringsOptions settings = sp.GetRequiredService<IOptions<ConnectionStringsOptions>>().Value;
            options.UseSqlServer(settings.Database);
            options.UseSnakeCaseNamingConvention();
        });
        services.AddTransient<IdentityServerDbContextSeeder>();

        // Identity
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
        .AddEntityFrameworkStores<IdentityServerDbContext>()
        .AddDefaultTokenProviders();
    }
}
