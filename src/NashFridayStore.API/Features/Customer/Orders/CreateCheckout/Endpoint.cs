using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.Domain.Commons;

namespace NashFridayStore.API.Features.Customer.Orders.CreateCheckout;

[Authorize(Roles = AppCts.Identity.Roles.Customer)]
[ApiController]
[Route("api/customer/orders/checkout")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    /// <summary>
    /// Create a checkout session for payment
    /// </summary>
    [HttpPost]
    [Tags("Customer - Orders")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Post(
        [FromBody] Request request,
        CancellationToken cancellationToken)
    {
        Response response = await handler.HandleAsync(request, cancellationToken);
        return Ok(response);
    }
}
