using Microsoft.AspNetCore.Mvc;
using NashFridayStore.SharedFeatures.Features.Categories.GetCategories;

namespace NashFridayStore.API.Endpoints.Categories;

[ApiController]
[Route("api/categories")]
internal sealed class GetCategoriesEndpoint(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] Request request,
        CancellationToken ct
    )
    {
        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
