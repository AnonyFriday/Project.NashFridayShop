using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.API.Features.All.Products.GetProducts;

[AllowAnonymous]
[ApiController]
[Route("api/all/products")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    /// <summary>
    /// Get all products with pagination and filtering
    /// </summary>
    [HttpGet]
    [Tags("Public - Products")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(
        [FromQuery] Request request,
        CancellationToken ct
    )
    {
        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
