using Microsoft.AspNetCore.Mvc;
using NashFridayStore.SharedFeatures.Features.Products.GetProductRatings;

namespace NashFridayStore.API.Endpoints.Admin.Products;

[ApiController]
[Route("api/admin/products/{productId:guid}/ratings")]
public sealed class GetProductRatingsEndpoint(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] Guid productId, [FromQuery] Request request, CancellationToken ct)
    {
        Request req = request with { ProductId = productId };
        Response response = await handler.Handle(req, ct);
        return Ok(response);
    }
}
