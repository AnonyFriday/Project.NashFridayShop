using NashFridayStore.API.Extensions;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.Infrastructure.Extensions;

// Configure services

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

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

app.UseHttpsRedirection();
app.UseCors(AppCts.Policy.AdminSite);

app.MapControllers();

await app.RunAsync();

