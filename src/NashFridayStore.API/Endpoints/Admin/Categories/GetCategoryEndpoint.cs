using Microsoft.AspNetCore.Mvc;
using NashFridayStore.API.Features.Categories.GetCategory;
using NashFridayStore.SharedFeatures.Features.Categories.GetCategory;

namespace NashFridayStore.API.Endpoints.Admin.Categories;

[ApiController]
[Route("api/admin/categories/{id:guid}")]
public sealed class GetCategoryEndpoint(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken ct)
    {
        var request = new Request(id);
        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
