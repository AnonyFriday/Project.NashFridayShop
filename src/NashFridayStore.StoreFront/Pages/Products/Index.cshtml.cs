using Microsoft.AspNetCore.Mvc;
using NashFridayStore.StoreFront.Services.Categories;
using NashFridayStore.StoreFront.Services.Products;
using NashFridayStore.StoreFront.Services.Cart;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NashFridayStore.StoreFront.Pages.Products;

[BindProperties(SupportsGet = true)]
public class IndexModel(
    IProductApiClient productApiClient,
    ICategoryApiClient categoryApiClient,
    ICartApiClient cartApiClient) : BasePageModel
{
    [BindNever]
    public GetProducts.Response? ProductResponse { get; set; }

    [BindNever]
    public GetCategories.Response? CategoryResponse { get; set; }

    public Guid? CategoryId { get; set; }
    public string? SearchName { get; set; }
    public SortBy? SortBy { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 8;

    public async Task OnGetAsync()
    {
        // Cleaning Requests
        if (MinPrice > MaxPrice)
        {
            (MinPrice, MaxPrice) = (null, null);
        }

        CategoryResponse = await categoryApiClient.GetCategoriesAsync(new GetCategories.Request(IsAll: true));

        var request = new GetProducts.Request(
            SearchName: string.IsNullOrWhiteSpace(SearchName) ? null : SearchName,
            CategoryId: CategoryId,
            MinPrice: MinPrice,
            MaxPrice: MaxPrice,
            SortBy: SortBy,
            PageIndex: PageIndex,
            PageSize: PageSize
        );

        ProductResponse = await productApiClient.GetProductsAsync(request);
    }

    public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
    {
        GetProduct.Response? product = await productApiClient.GetProductByIdAsync(new GetProduct.Request(productId));
        if (product == null)
        {
            return NotFound();
        }

        await cartApiClient.CreateOrAddItemAsync(new CreateOrAddItemToCart.Request(
            ProductId: productId,
            Quantity: 1,
            ProductName: product.Name,
            ImageUrl: product.ImageUrl,
            Price: product.PriceUsd
        ));

        if (Request.Headers.ContainsKey("HX-Request"))
        {
            // special header from HTMX, to find and triggr async on cart element
            Response.Headers.Append("HX-Trigger", "cart-updated");
            return new StatusCodeResult(204);
        }

        return RedirectToPage();
    }
}
