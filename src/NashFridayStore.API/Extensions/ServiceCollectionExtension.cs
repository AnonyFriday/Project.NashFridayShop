using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation;
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
        serviceCollection.AddControllers()
        .AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

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

        // Fluent Validation
        serviceCollection.AddValidatorsFromAssembly(typeof(ServiceCollectionExtension).Assembly);

        // All Handlers
        RegisterAllFeatureHandlers(serviceCollection);
    }

    private static void RegisterAllFeatureHandlers(IServiceCollection serviceCollection)
    {
        Assembly assembly = typeof(ServiceCollectionExtension).Assembly;

        IEnumerable<Type> handlers = assembly
            .GetTypes()
            .Where(t => t.Name == "Handler" && t.IsClass && !t.IsAbstract && !t.IsInterface);

        foreach (Type handler in handlers)
        {
            serviceCollection.AddScoped(handler);
        }
    }
}
