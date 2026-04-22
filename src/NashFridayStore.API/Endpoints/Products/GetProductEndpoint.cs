using Microsoft.AspNetCore.Mvc;
using NashFridayStore.SharedFeatures.Features.Products.GetProduct;

namespace NashFridayStore.API.Endpoints.Products;

[ApiController]
[Route("api/products/{id:guid}")]
internal sealed class GetProductEndpoint(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken ct)
    {
        var request = new Request(id);
        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
