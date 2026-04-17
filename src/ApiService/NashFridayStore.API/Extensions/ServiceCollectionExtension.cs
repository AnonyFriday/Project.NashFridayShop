using NashFridayStore.API.ExceptionHandlers;
using NashFridayStore.API.Exceptions;

namespace NashFridayStore.API.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApiServices(this IServiceCollection serviceCollection)
    {
        // Exception Handlers 
        serviceCollection.AddProblemDetails();
        serviceCollection.AddExceptionHandler<GeneralExceptionHandler>();
        serviceCollection.AddExceptionHandler<InternalServerErrorExceptionHandler>();
    }
}
