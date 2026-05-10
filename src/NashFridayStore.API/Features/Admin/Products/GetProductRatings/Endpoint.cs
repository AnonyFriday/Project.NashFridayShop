using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.Domain.Commons;

namespace NashFridayStore.API.Features.Admin.Products.GetProductRatings;

[Authorize(Roles = AppCts.Identity.Roles.Admin)]
[ApiController]
[Route("api/admin/products/{productId:guid}/ratings")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] Guid productId, [FromQuery] Request request, CancellationToken ct)
    {
        Request req = request with { ProductId = productId };
        Response response = await handler.Handle(req, ct);
        return Ok(response);
    }
}
