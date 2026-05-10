using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.Domain.Commons;

namespace NashFridayStore.API.Features.Customer.Cart.GetCart;

[Authorize(Roles = AppCts.Identity.Roles.Customer)]
[ApiController]
[Route("api/customer/cart")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        Response response = await handler.HandleAsync(ct);
        return Ok(response);
    }
}
