using Microsoft.AspNetCore.Mvc;
using NashFridayStore.SharedFeatures.Features.Products.GetProducts;

namespace NashFridayStore.API.Endpoints.Admin.Products;

[ApiController]
[Route("api/admin/products")]
public sealed class GetProductsEndpoint(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] Request request,
        CancellationToken ct
    )
    {
        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
