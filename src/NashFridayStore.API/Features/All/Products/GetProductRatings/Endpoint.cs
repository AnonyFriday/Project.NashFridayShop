using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.API.Features.All.Products.GetProductRatings;

[Tags("Public - Products")]
[AllowAnonymous]
[ApiController]
[Route("api/all/products/{productId:guid}/ratings")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    /// <summary>
    /// Get ratings and comments for a product
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] Guid productId, [FromQuery] Request request, CancellationToken ct)
    {
        Request req = request with { ProductId = productId };
        Response response = await handler.Handle(req, ct);
        return Ok(response);
    }
}
