using NashFridayStore.IdentityServer.Data;
using NashFridayStore.IdentityServer.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddServices(builder.Configuration);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    using IServiceScope scope = app.Services.CreateScope();
    IdentityServerDbContextSeeder seeder = scope.ServiceProvider.GetRequiredService<IdentityServerDbContextSeeder>();
    await seeder.SeedAsync();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

await app.RunAsync();
