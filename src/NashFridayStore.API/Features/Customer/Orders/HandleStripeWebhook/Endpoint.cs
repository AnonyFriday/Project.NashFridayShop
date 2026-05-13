using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.API.Features.Customer.Orders.HandleStripeWebhook;

[AllowAnonymous]
[ApiController]
[Route("api/customer/orders/webhook")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    /// <summary>
    /// Handle Stripe webhooks for order status updates
    /// </summary>
    [HttpPost]
    [Tags("Customer - Orders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(
        [FromHeader(Name = "Stripe-Signature")] string? stripeSignature,
        CancellationToken ct)
    {
        // get the raw json body as a string to verify the stripe-signatureee
        string json;
        using (var reader = new StreamReader(Request.Body))
        {
            json = await reader.ReadToEndAsync(ct);
        }

        var request = new Request(json, stripeSignature);
        Response response = await handler.HandleAsync(request, ct);

        return Ok(response);
    }
}
