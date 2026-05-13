using NashFridayStore.StoreFront.Extensions;
using NashFridayStore.StoreFront.Middlewares;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddStoreFrontServices(builder.Configuration);

WebApplication app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePagesWithReExecute("/Errors/{0}");

app.UseHsts();
app.UseHttpsRedirection();

app.UseRouting();

app.UseMiddleware<SetLoggedInUserMiddleware>();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

await app.RunAsync();
