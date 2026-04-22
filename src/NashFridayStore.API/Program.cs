using NashFridayStore.API.Extensions;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.Infrastructure.Extensions;
using NashFridayStore.SharedFeatures.Extensions;

// Configure services

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddSharedFeatures();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    using IServiceScope scope = app.Services.CreateScope();
    StoreDbContextSeeder seeder = scope.ServiceProvider.GetRequiredService<StoreDbContextSeeder>();
    await seeder.SeedAsync();
}

if (app.Environment.IsEnvironment(AppCts.Environment.Testing))
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors(AppCts.Policy.AdminSite);

app.MapControllers();

await app.RunAsync();

