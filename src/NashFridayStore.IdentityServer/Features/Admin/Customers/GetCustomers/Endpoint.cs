using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.IdentityServer.Commons;

namespace NashFridayStore.IdentityServer.Features.Admin.Customers.GetCustomers;

[Authorize(Roles = AppCts.Identity.Roles.Admin)]
[ApiController]
[Route("api/admin/customers")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] Request request,
        CancellationToken ct)
    {
        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
