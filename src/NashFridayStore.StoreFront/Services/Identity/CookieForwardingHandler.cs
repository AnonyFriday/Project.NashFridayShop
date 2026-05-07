namespace NashFridayStore.StoreFront.Services;

public class CookieForwardingHandler(
    IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpContext? httpContext = httpContextAccessor.HttpContext;
        if (httpContext != null && httpContext.Request.Cookies.Any())
        {
            foreach (KeyValuePair<string, string> cookie in httpContext.Request.Cookies)
            {
                Console.WriteLine($"Forwarding cookie {cookie.Key} = {cookie.Value}");
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
