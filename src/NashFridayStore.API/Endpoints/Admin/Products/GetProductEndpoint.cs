using Microsoft.AspNetCore.Mvc;
using NashFridayStore.SharedFeatures.Features.Products.GetProduct;

namespace NashFridayStore.API.Endpoints.Admin.Products;

[ApiController]
[Route("api/admin/products/{id:guid}")]
public sealed class GetProductEndpoint(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken ct)
    {
        var request = new Request(id);
        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
