using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.Domain.Commons;

namespace NashFridayStore.API.Features.Customer.Cart.AddCart;

[Authorize(Roles = AppCts.Identity.Roles.Customer)]
[ApiController]
[Route("api/customer/cart")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Request request, CancellationToken ct)
    {
        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
