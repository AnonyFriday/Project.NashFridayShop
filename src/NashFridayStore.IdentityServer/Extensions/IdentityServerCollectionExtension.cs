using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NashFridayStore.IdentityServer.AppOptions;
using NashFridayStore.IdentityServer.Data;
using NashFridayStore.IdentityServer.Domain;
using OpenIddict.Abstractions;

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

        // Configure Cookies for Authentication
        services.ConfigureApplicationCookie(otp =>
        {
            otp.LoginPath = "/Account/Login";
        });

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
                // opt.AllowClientCredentialsFlow().AllowRefreshTokenFlow(); // Machine to Machine authentication
                // opt.AllowPasswordFlow().AllowRefreshTokenFlow();          // User type username and password

                opt.AllowAuthorizationCodeFlow().AllowRefreshTokenFlow(); // Communicate with BFF via authorization code
                opt.RequireProofKeyForCodeExchange(); // code verifier for BFF

                // add scope
                opt.RegisterScopes(
                    OpenIddictConstants.Scopes.OpenId,
                    OpenIddictConstants.Scopes.Profile,
                    OpenIddictConstants.Scopes.Email,
                    OpenIddictConstants.Scopes.Roles,
                    "api" // my api scope
                );

                // Using dev cert, but disable access token  encryption for learning purpose
                opt
                    .AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate()
                    .DisableAccessTokenEncryption();

                // I manually handle those process via endpoint
                opt
                    .UseAspNetCore() // go through
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableTokenEndpointPassthrough()
                    .EnableEndSessionEndpointPassthrough()
                    .DisableTransportSecurityRequirement(); // using in local dev for now

                // Setup token, authorize, logout for our own
                opt.SetAuthorizationEndpointUris("/connect/authorize");
                opt.SetTokenEndpointUris("/connect/token");
                opt.SetEndSessionEndpointUris("/connect/logout");
            });

        // Add Razor Page + Controller Endpoints
        services.AddRazorPages();
        services.AddControllers();

        // Fluent Validation
        services.AddValidatorsFromAssembly(typeof(IdentityServerCollectionExtension).Assembly);

        // Add all handlers
        RegisterAllFeatureHandlers(services);
    }

    private static void RegisterAllFeatureHandlers(IServiceCollection serviceCollection)
    {
        Assembly assembly = typeof(IdentityServerCollectionExtension).Assembly;

        IEnumerable<Type> handlers = assembly
            .GetTypes()
            .Where(t => t.Name == "Handler" && t.IsClass && !t.IsInterface && !t.IsAbstract);

        foreach (Type handler in handlers)
        {
            serviceCollection.AddScoped(handler);
        }
    }
}
