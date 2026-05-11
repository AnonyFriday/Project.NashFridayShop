namespace NashFridayStore.API.Features.Customer.Orders.CreateCheckout;

public sealed record Request(string CustomerName,
    string CustomerEmail,
    string DeliveryAddress,
    string PhoneNumber);
