using NashFridayStore.API.Extensions;
using NashFridayStore.Infrastructure.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddApiServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

await app.RunAsync();

