using Microsoft.Extensions.Options;
using NashFridayStore.StoreFront.AppOptions;
using NashFridayStore.StoreFront.Services;
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

        // Add API Settings from appsettings.json
        services.AddOptions<ApiUrlOptions>()
            .Bind(configuration.GetSection(ApiUrlOptions.ApiSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Add CookieForwardingHandler
        services.AddTransient<CookieForwardingHandler>();

        // Add NashFridayApiClient for HttpClient
        services.AddHttpClient<BaseApiClient>((sp, client) =>
        {
            ApiUrlOptions options = sp.GetRequiredService<IOptions<ApiUrlOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseApiUrl);
        }).AddHttpMessageHandler<CookieForwardingHandler>();

        // Register domain http client api
        services.AddScoped<ICategoryApiClient, CategoryApiClient>();
        services.AddScoped<IProductApiClient, ProductApiClient>();
        services.AddScoped<IIdentityApiClient, IdentityApiClient>();
    }
}
