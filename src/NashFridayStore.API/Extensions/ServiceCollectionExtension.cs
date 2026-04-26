using Microsoft.Extensions.Options;
using NashFridayStore.API.ExceptionHandlers;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Infrastructure.AppOptions;

namespace NashFridayStore.API.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApiServices(this IServiceCollection serviceCollection)
    {
        // Exception Handlers 
        serviceCollection.AddProblemDetails();
        serviceCollection.AddExceptionHandler<GeneralExceptionHandler>();
        serviceCollection.AddExceptionHandler<InternalServerErrorExceptionHandler>();

        // Controllers 
        serviceCollection.AddControllers();

        // Documentation
        serviceCollection.AddOpenApi();

        // CORS
        SiteUrlsOption SiteUrls = serviceCollection.BuildServiceProvider().GetRequiredService<IOptions<SiteUrlsOption>>().Value;

        serviceCollection.AddCors(options =>
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
    }
}
