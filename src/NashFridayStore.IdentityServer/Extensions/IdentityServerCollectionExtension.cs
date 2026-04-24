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

            // Inject Open Iddict tables
            options.UseOpenIddict();
        });
        services.AddTransient<IdentityServerDbContextSeeder>();

        // Identity
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
        .AddEntityFrameworkStores<IdentityServerDbContext>()
        .AddDefaultTokenProviders();

        // Add OpenIddict
        services
            .AddOpenIddict()
            .AddCore(opt =>
            {
                // storing tokens, scope, authorization, clients
                opt.UseEntityFrameworkCore().UseDbContext<IdentityServerDbContext>();
            })
            .AddServer(opt =>
            {
                // enable password flow client credentials
                opt.AllowClientCredentialsFlow().AllowRefreshTokenFlow(); // Machine to Machine authentication
                opt.AllowPasswordFlow().AllowRefreshTokenFlow();          // User type username and password

                // Using dev cert, but disable access token  encryption for learning purpose
                opt
                    .AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate()
                    .DisableAccessTokenEncryption();

                // Enable those option, I will authenticate, OpenIddict issue token only
                opt
                    .UseAspNetCore() // go through
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableTokenEndpointPassthrough()
                    .EnableEndSessionEndpointPassthrough()
                    .DisableTransportSecurityRequirement();

                // Setup token, authorize, logout for our own
                opt.SetAuthorizationEndpointUris("/connect/authorize");
                opt.SetTokenEndpointUris("/connect/token");
                opt.SetEndSessionEndpointUris("/connect/logout");

            });
    }
}
