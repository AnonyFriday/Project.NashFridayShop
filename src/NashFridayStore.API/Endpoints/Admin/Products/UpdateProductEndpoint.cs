using Microsoft.AspNetCore.Mvc;
using NashFridayStore.SharedFeatures.Features.Products.UpdateProduct;

namespace NashFridayStore.API.Endpoints.Admin.Products;

[ApiController]
[Route("api/admin/products/{id:guid}")]
public sealed class UpdateProductEndpoint(Handler handler) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> Put(
        [FromRoute] Guid id,
        [FromBody] RequestBody body,
        CancellationToken ct
    )
    {
        var request = new Request(
            id,
            new RequestBody(
                body.CategoryId,
                body.Name,
                body.Description,
                body.PriceUsd,
                body.ImageUrl,
                body.Quantity,
                body.Status
            ));

        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
