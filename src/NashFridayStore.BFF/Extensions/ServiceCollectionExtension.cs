using System.Collections;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using NashFridayStore.BFF.AppOptions;
using Yarp.ReverseProxy.Configuration;

namespace NashFridayStore.BFF.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Settings at appsettings.json
        services.AddOptions<SiteUrlsOption>()
            .Bind(configuration.GetSection(SiteUrlsOption.SiteUrls))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Add Cookie as default authen mechanism for BFF
        // Add OpenIdConnect to handle redirecting to OIDC
        services.AddAuthentication(otp =>
        {
            otp.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            otp.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, otp =>
        {
            otp.Cookie.Name = "NashFridayStore.BFF.Session";
            otp.Cookie.HttpOnly = true;
            otp.Cookie.SameSite = SameSiteMode.Lax;
            otp.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        })
        .AddOpenIdConnect();

        // Configure OpenIdConnect with options
        services
            .AddOptions<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme)
            .Configure<IOptions<SiteUrlsOption>>((opt, siteUrlsOtp) =>
            {
                IdentityServerOptions identityServerOpts = siteUrlsOtp.Value.IdentityServer;

                opt.Authority = identityServerOpts.Authority;
                opt.ClientId = identityServerOpts.ClientId;
                opt.ClientSecret = identityServerOpts.ClientSecret;
                opt.CallbackPath = identityServerOpts.SignInCallbackPath;
                opt.SignedOutCallbackPath = identityServerOpts.SignOutCallbackPath;
                opt.ResponseType = OpenIdConnectResponseType.Code;
                opt.SaveTokens = true;
                opt.UsePkce = true;
                opt.RequireHttpsMetadata = false; // disable for development only

                // BFF requires claims based on supported scope from identity server
                opt.Scope.Add(OpenIdConnectScope.OpenId);
                opt.Scope.Add(OpenIdConnectScope.Profile);
                opt.Scope.Add(OpenIdConnectScope.Email);
                opt.Scope.Add(OpenIdConnectScope.OfflineAccess);
                opt.Scope.Add(identityServerOpts.ApiScope);
            });

        // Add Authorization
        services.AddAuthorization();

        // Add Controllers
        services.AddControllers();

        // Add HttpContext
        services.AddHttpContextAccessor();

        // Register Handlers
        RegisterAllFeatureHandlers(services);

        // Add Reverse Proxy
        RegisterReverseProxy(services, configuration);
    }

    private static void RegisterReverseProxy(IServiceCollection services, IConfiguration configuration)
    {
        SiteUrlsOption? siteUrls = configuration.GetSection(SiteUrlsOption.SiteUrls).Get<SiteUrlsOption>();
        string apiServerUrl = siteUrls!.ApiServer.Url;

        services.AddReverseProxy()
            .LoadFromMemory(
                routes: [
                    new RouteConfig() {
                        RouteId = "api-route",
                        ClusterId = "api-cluster",
                        Match = new RouteMatch() {
                            Methods = ["GET", "POST", "PUT", "DELETE", "PATCH"],
                            Path = "/api/{**catch-all}" // catch all request with /api/products, /api/categories,..
                        }
                    }
                ],
                clusters: [
                    new ClusterConfig() {
                        ClusterId = "api-cluster",
                        Destinations = new Dictionary<string, DestinationConfig>() {
                            {
                                "api-destination",
                                new DestinationConfig() {
                                    Address = apiServerUrl,
                                }
                            }
                        }
                    }
                ]
            );
    }

    private static void RegisterAllFeatureHandlers(IServiceCollection services)
    {
        Assembly assembly = typeof(ServiceCollectionExtension).Assembly;

        IEnumerable<Type> handlers = assembly
            .GetTypes()
            .Where(t => t.Name == "Handler" && t.IsClass && !t.IsInterface && !t.IsAbstract);

        foreach (Type handler in handlers)
        {
            services.AddScoped(handler);
        }
    }
}
