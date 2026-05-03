using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.API.Features.Products.GetProduct;

[ApiController]
[Route("api/products/{id:guid}")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromRoute] Guid id,
        [FromQuery] bool IncludeDeleted, CancellationToken ct)
    {
        var request = new Request(id, IncludeDeleted);
        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
