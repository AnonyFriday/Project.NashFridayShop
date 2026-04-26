using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.API.Features.Products.GetProductRatings;

[ApiController]
[Route("api/products/{productId:guid}/ratings")]
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
