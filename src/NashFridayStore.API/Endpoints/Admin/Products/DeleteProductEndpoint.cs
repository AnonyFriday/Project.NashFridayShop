using Microsoft.AspNetCore.Mvc;
using NashFridayStore.SharedFeatures.Features.Products.DeleteProduct;

namespace NashFridayStore.API.Endpoints.Admin.Products;

[ApiController]
[Route("api/admin/products/{id:guid}")]
public sealed class DeleteProductEndpoint(Handler handler) : ControllerBase
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
