using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.API.Features.Products.UpdateProduct;

[ApiController]
[Route("api/products/{id:guid}")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> Put(
        [FromRoute] Guid id,
        [FromQuery] bool includeDeleted,
        [FromBody] RequestBody body,
        CancellationToken ct
    )
    {
        var request = new Request(
            id,
            body,
            includeDeleted);

        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
