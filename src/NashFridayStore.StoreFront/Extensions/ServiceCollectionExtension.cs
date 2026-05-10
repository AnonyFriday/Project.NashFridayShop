using Microsoft.Extensions.Options;
using NashFridayStore.StoreFront.AppOptions;
using NashFridayStore.StoreFront.Interceptors;
using NashFridayStore.StoreFront.Services;
using NashFridayStore.StoreFront.Services.Cart;
using NashFridayStore.StoreFront.Services.Categories;
using NashFridayStore.StoreFront.Services.Identity;
using NashFridayStore.StoreFront.Services.Products;

namespace NashFridayStore.StoreFront.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddStoreFrontServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Razor Page
        services.AddRazorPages();

        // Add HttpContextAccessor for HttpContext
        services.AddHttpContextAccessor();

        // Configure Antiforgery for HTMX/AJAX
        services.AddAntiforgery(options =>
        {
            options.HeaderName = "RequestVerificationToken";
        });

        // Add API Settings from appsettings.json
        services.AddOptions<ApiUrlOptions>()
            .Bind(configuration.GetSection(ApiUrlOptions.ApiSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Add Delegating Handlers for outbound requests
        services.AddTransient<CookieForwardingDelegatingHandler>();

        // Add NashFridayApiClient for HttpClient
        services.AddHttpClient<BaseApiClient>((sp, client) =>
        {
            ApiUrlOptions options = sp.GetRequiredService<IOptions<ApiUrlOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseApiUrl);
        }).AddHttpMessageHandler<CookieForwardingDelegatingHandler>();

        // Register domain http client api
        services.AddScoped<ICategoryApiClient, CategoryApiClient>();
        services.AddScoped<IProductApiClient, ProductApiClient>();
        services.AddScoped<IAccountApiClient, AccountApiClient>();
        services.AddScoped<ICartApiClient, CartApiClient>();
    }
}
