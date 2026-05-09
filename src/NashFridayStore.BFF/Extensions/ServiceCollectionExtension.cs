using System.Net.Http.Headers;
using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using NashFridayStore.BFF.AppOptions;
using NashFridayStore.BFF.Commons;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Transforms;

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
                opt.MapInboundClaims = false; // fixed claim been renamed into the legacy name (xml schema)

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

        // Add Cors
        SiteUrlsOption SiteUrls = services.BuildServiceProvider().GetRequiredService<IOptions<SiteUrlsOption>>().Value;

        services.AddCors(options =>
        {
            options.AddPolicy(AppCts.Policy.AdminSite, policy =>
            {
                if (SiteUrls.AdminUrls.Length > 0)
                {
                    policy.WithOrigins(SiteUrls.AdminUrls)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                }
            });
        });

        // Register Handlers
        RegisterAllFeatureHandlers(services);

        // Add Reverse Proxy
        RegisterReverseProxy(services, configuration);
    }

    private static void RegisterReverseProxy(IServiceCollection services, IConfiguration configuration)
    {
        SiteUrlsOption? siteUrls = configuration.GetSection(SiteUrlsOption.SiteUrls).Get<SiteUrlsOption>();
        string apiServerUrl = siteUrls!.ApiServer.Url;
        string identityServerUrl = siteUrls.IdentityServer.Authority;

        services.AddReverseProxy()
            .AddTransforms(context =>
            {
                // Decrypt the cookie schema
                context.AddRequestTransform(async contextTransform =>
                {
                    string? accessToken = await contextTransform.HttpContext.GetTokenAsync("access_token");
                    if (!string.IsNullOrWhiteSpace(accessToken))
                    {
                        // attach bearer token to subsequent request
                        contextTransform.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue(
                            "Bearer", accessToken
                        );
                    }
                });
            })
            .LoadFromMemory(
                routes: [
                    new RouteConfig() {
                        RouteId = "admin-identity-route",
                        ClusterId = "identity-cluster",
                        Match = new RouteMatch() {
                            Methods = ["GET", "POST", "PUT", "DELETE", "PATCH"],
                            Path = "/api/admin/customers/{**catch-all}"
                        }
                    },

                    new RouteConfig() {
                        RouteId = "admin-api-route",
                        ClusterId = "api-cluster",
                        Match = new RouteMatch() {
                            Methods = ["GET", "POST", "PUT", "DELETE", "PATCH"],
                            Path = "/api/admin/{**catch-all}" // catch all request with /api/admin/products, /api/admin/categories,..
                        }
                    },

                    new RouteConfig() {
                        RouteId = "customer-api-route",
                        ClusterId = "api-cluster",
                        Match = new RouteMatch() {
                            Methods = ["GET", "POST", "PUT", "PATCH"], // do not allow delete in any cases
                            Path = "/api/customer/{**catch-all}"
                        }
                    },

                    new RouteConfig() {
                        RouteId = "all-api-route",
                        ClusterId = "api-cluster",
                        Match = new RouteMatch() {
                            Methods = ["GET"],
                            Path = "/api/all/{**catch-all}" // Only allow views for public api
                        }
                    },
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
                    },

                    new ClusterConfig() {
                        ClusterId = "identity-cluster",
                        Destinations = new Dictionary<string, DestinationConfig>() {
                            {
                                "identity-destination",
                                new DestinationConfig() {
                                    Address = identityServerUrl,
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
