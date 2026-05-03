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
    await seeder.SeedAccountsAsync();
    await seeder.SeedBff();
    await seeder.SeedOidcDebugger();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.MapRazorPages();
app.MapControllers();

await app.RunAsync();
