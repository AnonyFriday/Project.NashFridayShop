using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using NashFridayStore.StoreFront.AppOptions;
using NashFridayStore.StoreFront.Services;
using NashFridayStore.StoreFront.Services.Categories;

namespace NashFridayStore.StoreFront.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddStoreFrontServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Razor Pages
        services.AddRazorPages();

        // Add HttpContextAccessor for HttpContext
        services.AddHttpContextAccessor();

        // Add API Settings from appsettings.json
        services.AddOptions<ApiUrlOptions>()
            .Bind(configuration.GetSection(ApiUrlOptions.ApiSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Add NashFridayApiClient for HttpClient
        services.AddHttpClient<BaseApiClient>((sp, client) =>
#pragma warning disable S125 // Sections of code should not be commented out

        {
            ApiUrlOptions options = sp.GetRequiredService<IOptions<ApiUrlOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseApiAddress);

            // HttpContext? httpContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
            // if (httpContext != null && httpContext.Request.Headers.TryGetValue("Cookie", out StringValues cookies))
            // {
            //     client.DefaultRequestHeaders.Add("Cookie", cookies.ToString());
            // }
        }
#pragma warning restore S125 // Sections of code should not be commented out
);

        // Register domain http client api
        services.AddScoped<ICategoryApiClient, CategoryApiClient>();
    }
}
