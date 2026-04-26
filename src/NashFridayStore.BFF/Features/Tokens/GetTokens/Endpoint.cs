using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.BFF.Features.Tokens.GetTokens;

// THIS API IS ONLY FOR DEVELOPMENT TESTING
// DO NOT CONSUME
[ApiController]
[Route("dev/api/tokens")]
public class Endpoint(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        Response response = await handler.HandleAsync(ct);
        return Ok(response);
    }
}
