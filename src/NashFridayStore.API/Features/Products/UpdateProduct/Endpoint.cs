using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.API.Features.Products.UpdateProduct;

[ApiController]
[Route("api/products/{id:guid}")]
public sealed class Endpoint(Handler handler) : ControllerBase
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
