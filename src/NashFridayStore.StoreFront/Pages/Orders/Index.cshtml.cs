using Microsoft.AspNetCore.Mvc;
using NashFridayStore.StoreFront.Services.Orders;

namespace NashFridayStore.StoreFront.Pages.Orders;

public class IndexModel(IOrderApiClient orderApiClient) : BasePageModel
{
    public List<GetMyOrders.OrderItemResponse> Orders { get; private set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        GetMyOrders.Response? response = await orderApiClient.GetMyOrdersAsync();

        if (response != null)
        {
            Orders = response.Orders;
        }

        return Page();
    }
}
