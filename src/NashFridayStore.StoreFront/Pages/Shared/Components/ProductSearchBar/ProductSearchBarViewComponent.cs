using Microsoft.AspNetCore.Mvc;
using NashFridayStore.StoreFront.Services.Products;

namespace NashFridayStore.StoreFront.Pages.Shared.Components.ProductSearchBar;

public class ProductSearchBarViewComponent(IProductApiClient productApiClient) : ViewComponent
{
    public ProductSearchBarResponseVM ProductSearchBarResponseVM { get; set; } = new();

    public async Task<IViewComponentResult> InvokeAsync(ProductSearchBarRequestVM? orgReq = null)
    {
        orgReq ??= new ProductSearchBarRequestVM(null);

        // Clean request from view
        ProductSearchBarRequestVM req = orgReq with
        {
            SearchName = string.IsNullOrWhiteSpace(orgReq.SearchName) ? null : orgReq.SearchName,
        };

        // Call API request and binding back result to response VM for view
        GetProducts.Response? response = await productApiClient.GetProductsAsync(
            new GetProducts.Request(req.SearchName, null, null, null, null, null, 0, 5));

        ProductSearchBarResponseVM.Products = response?.Items
                    .Select(p => new ProductSearchBarResponseVM.Product(p.Id.ToString(), p.Name, p.CategoryName ?? "", p.ImageUrl))
                    .ToList()
                    ?? new List<ProductSearchBarResponseVM.Product>();

        ProductSearchBarResponseVM.SearchName = req.SearchName ?? string.Empty;

        return View(ProductSearchBarResponseVM);
    }
}
