namespace NashFridayStore.StoreFront.Services.Orders;

public static class CreateCheckout
{
    public record Request(
        string CustomerName,
        string CustomerEmail,
        string DeliveryAddress,
        string PhoneNumber);

    public record Response(string CheckoutUrl);
}
