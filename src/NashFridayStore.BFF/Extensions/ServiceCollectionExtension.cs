using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using NashFridayStore.BFF.AppOptions;

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
            .ValidateOnStart();

        // Add Cookie as default authen mechanism for BFF
        // Add OpenIdConnect to handle redirecting to OIDC
        services.AddAuthentication(otp =>
        {
            otp.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            otp.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie(otp =>
        {
            otp.Cookie.Name = "NashFridayStore.BFF.Session";
            otp.Cookie.HttpOnly = true;
            otp.Cookie.SameSite = SameSiteMode.Lax;
            otp.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        })
        .AddOpenIdConnect();

        // Add Authorization
        services.AddAuthorization();

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
                opt.UsePkce = true;
                opt.RequireHttpsMetadata = false; // disable for development only

                opt.Scope.Add(OpenIdConnectScope.OpenId);
                opt.Scope.Add(OpenIdConnectScope.Profile);
                opt.Scope.Add(OpenIdConnectScope.Email);
                opt.Scope.Add(identityServerOpts.ApiScope);
            });

#pragma warning disable S125 // Sections of code should not be commented out
        // opt.SaveTokens();
        // otp.Scope.Clear();
        // otp.Scope.Add(identityServiceOptions.Roles);
        // otp.Scope.Add(OpenIdConnectScope.OfflineAccess);
#pragma warning restore S125 // Sections of code should not be commented out

        // Add Controllers
        services.AddControllers();
    }
}
