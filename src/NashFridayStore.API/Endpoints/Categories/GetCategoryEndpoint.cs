using Microsoft.AspNetCore.Mvc;
using NashFridayStore.API.Features.Categories.GetCategory;
using NashFridayStore.SharedFeatures.Features.Categories.GetCategory;

namespace NashFridayStore.API.Endpoints.Categories;

[ApiController]
[Route("api/categories/{id:guid}")]
internal sealed class GetCategoryEndpoint(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken ct)
    {
        var request = new Request(id);
        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
