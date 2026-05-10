using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.Domain.Commons;

namespace NashFridayStore.API.Features.Customer.Cart.CreateOrAddItemToCart;

[Authorize(Roles = AppCts.Identity.Roles.Customer)]
[ApiController]
[Route("api/customer/cart")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Request request, CancellationToken ct)
    {
        Response response = await handler.HandleAsync(request, ct);
        return CreatedAtAction(nameof(Post), new { id = response.Id }, response);
    }
}
