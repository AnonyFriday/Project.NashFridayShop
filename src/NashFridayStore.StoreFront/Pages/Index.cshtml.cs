using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NashFridayStore.StoreFront.Services.Products;

namespace NashFridayStore.StoreFront.Pages;

public class IndexModel(IProductApiClient productApiClient) : PageModel
{
    private const int _topRatedCount = 10;

    #region Bind Properties

    [BindProperty(SupportsGet = true)]
    public IList<GetTopRatedProducts.ProductItem> TopProducts { get; set; }
    #endregion

    public async Task OnGetAsync()
    {
        GetTopRatedProducts.Response? response = await productApiClient.GetTopRatedProductsAsync(new GetTopRatedProducts.Request(_topRatedCount));
        TopProducts = response?.Items ?? [];
    }
}
