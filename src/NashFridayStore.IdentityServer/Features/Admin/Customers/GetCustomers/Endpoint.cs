using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.IdentityServer.Commons;

namespace NashFridayStore.IdentityServer.Features.Admin.Customers.GetCustomers;

// quick fix if do not set global JWT schema
// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = AppCts.Identity.Roles.Admin)]
[Authorize(Policy = AppCts.Identity.Auth.AdminPolicy, Roles = AppCts.Identity.Roles.Admin)]
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
