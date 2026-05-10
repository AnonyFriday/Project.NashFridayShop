using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using NashFridayStore.API.ExceptionHandlers;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Infrastructure.AppOptions;

namespace NashFridayStore.API.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApiServices(this IServiceCollection serviceCollection)
    {
        // Context 
        serviceCollection.AddHttpContextAccessor();

        // Settings at appsettings.json
        serviceCollection.AddOptions<IdentityServerOptions>()
            .BindConfiguration(IdentityServerOptions.IdentityServer)
            .ValidateOnStart();

        // Add JWT Token Handler for access_token from BFF and Authroization
        serviceCollection
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        serviceCollection.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<IdentityServerOptions>>((opt, idSettings) =>
            {
                IdentityServerOptions settings = idSettings.Value;

                opt.Authority = settings.Authority; // specify this to down the public key from the authority to decrypt the token
                opt.Audience = settings.Audience;

                opt.RequireHttpsMetadata = false;
                opt.MapInboundClaims = false;

                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateAudience = true,
                    NameClaimType = "name", // dont use XML ugly name as set to false, then specify the name here to check in claim
                    RoleClaimType = "role"
                };
            });
        serviceCollection.AddAuthorization();

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
