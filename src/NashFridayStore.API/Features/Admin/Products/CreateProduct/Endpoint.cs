using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.API.Features.Admin.Products.CreateProduct;

[ApiController]
[Route("api/admin/products")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] Request request,
        CancellationToken ct
    )
    {
        Response response = await handler.HandleAsync(request, ct);
        return CreatedAtAction(nameof(Post), new { id = response.Id }, response);
    }
}
