using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.API.Features.Products.DeleteProduct;

[ApiController]
[Route("api/products/{id:guid}")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    [HttpDelete]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken ct
    )
    {
        var request = new Request(id);
        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
