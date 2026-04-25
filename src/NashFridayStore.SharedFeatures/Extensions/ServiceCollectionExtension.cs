using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace NashFridayStore.SharedFeatures.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddSharedFeatures(this IServiceCollection serviceCollection)
    {
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
