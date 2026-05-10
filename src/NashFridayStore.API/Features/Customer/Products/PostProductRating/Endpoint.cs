using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.Domain.Commons;

namespace NashFridayStore.API.Features.Customer.Products.PostProductRating;

[Authorize(Roles = AppCts.Identity.Roles.Customer)]
[ApiController]
[Route("api/customer/products/{productId:guid}/rating")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(
        [FromRoute] Guid ProductId,
        [FromBody] RequestBody body,
        CancellationToken ct
    )
    {
        var request = new Request(
            ProductId,
            body);

        Response response = await handler.HandleAsync(request, ct);
        return CreatedAtAction(nameof(Post), response);
    }
}
