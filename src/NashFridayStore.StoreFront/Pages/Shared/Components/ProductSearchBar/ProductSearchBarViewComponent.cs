using Microsoft.AspNetCore.Mvc;
using NashFridayStore.StoreFront.Services.Products;

namespace NashFridayStore.StoreFront.Pages.Shared.Components.ProductSearchBar;

public class ProductSearchBarViewComponent(IProductApiClient productApiClient) : ViewComponent
{
    [BindProperty(SupportsGet = true)]
    public ProductSearchBarVM ProductSearchBarVM { get; set; } = new();

    public async Task<IViewComponentResult> InvokeAsync()
    {
        GetProducts.Response? response = await productApiClient.GetProductsAsync(new GetProducts.Request(null, null, null, null, null, null, 0, 5));
        ProductSearchBarVM.Products = response?.Items
                    .Select(p => new ProductSearchBarVM.Product(p.Id.ToString(), p.Name, p.CategoryName ?? "", p.ImageUrl))
                    .ToList()
                    ?? new List<ProductSearchBarVM.Product>();
        return View(ProductSearchBarVM);
    }
}
