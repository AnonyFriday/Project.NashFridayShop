using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NashFridayStore.IdentityServer.AppOptions;
using NashFridayStore.IdentityServer.Commons;
using NashFridayStore.IdentityServer.Commons.Exceptions;
using NashFridayStore.IdentityServer.ExceptionHandlers;
using NashFridayStore.IdentityServer.Data;
using NashFridayStore.IdentityServer.Domain;
using OpenIddict.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace NashFridayStore.IdentityServer.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        // Settings at appsettings.json
        services.AddOptions<ConnectionStringsOptions>()
            .Bind(configuration.GetSection(ConnectionStringsOptions.ConnectionStrings))
            .ValidateOnStart();

        services.AddOptions<SiteUrlsOption>()
            .Bind(configuration.GetSection(SiteUrlsOption.SiteUrls))
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

        // Add JwtBearer Token Handler for Admin API endpoints
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<SiteUrlsOption>>((opt, siteUrlsOtp) =>
            {
                SiteUrlsOption settings = siteUrlsOtp.Value;

                opt.Authority = settings.IdentityServerUrl;
                opt.Audience = settings.Bff.ApiServerAudience;

                opt.RequireHttpsMetadata = false;
                opt.MapInboundClaims = false;

                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateAudience = true,
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };

                opt.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse(); // skip default response and let me throw via exception handler
                        throw new UnauthorizedException();
                    },

                    OnForbidden = context =>
                    {
                        throw new ForbiddenException();
                    }
                };
            });
        services.AddAuthorization(options =>
        {
            // cannot override the default policy as JWT Bearer since the Login currently using cookie
            // create a explicit policy for endpoints that resolve JWT token via [Authorize]
            options.AddPolicy(AppCts.Identity.Auth.AdminPolicy, policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
            });
        });

        // Exception Handler
        services.AddProblemDetails();
        services.AddExceptionHandler<GeneralExceptionHandler>();
        services.AddExceptionHandler<InternalServerErrorExceptionHandler>();

        // Configure Cookies for Authentication
        services.ConfigureApplicationCookie(opt =>
        {
            opt.Cookie.Name = "NashFridayStore.Identity.LoginSession";
            opt.Cookie.HttpOnly = true;
            opt.Cookie.SameSite = SameSiteMode.Lax;
            opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            opt.LoginPath = "/Account/Login";
            opt.ExpireTimeSpan = TimeSpan.FromMinutes(AppCts.Identity.Auth.CookieTimeToLiveInMinutes);
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
                    OpenIddictConstants.Scopes.OfflineAccess,
                    "api" // my api scope
                );

                // Using dev cert, but disable access token  encryption for learning purpose
                opt
                    .AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate()
                    .DisableAccessTokenEncryption();

                // I manually handle those process via endpoint
                opt
                    .UseAspNetCore()
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
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtension).Assembly);

        // Add all handlers
        RegisterAllFeatureHandlers(services);
    }

    private static void RegisterAllFeatureHandlers(IServiceCollection serviceCollection)
    {
        Assembly assembly = typeof(ServiceCollectionExtension).Assembly;

        IEnumerable<Type> handlers = assembly
            .GetTypes()
            .Where(t => t.Name == "Handler" && t.IsClass && !t.IsInterface && !t.IsAbstract);

        foreach (Type handler in handlers)
        {
            serviceCollection.AddScoped(handler);
        }
    }
}
