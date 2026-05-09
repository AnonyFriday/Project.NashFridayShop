using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NashFridayStore.StoreFront.Services.Products;

namespace NashFridayStore.StoreFront.Pages.Products.Details;

[BindProperties(SupportsGet = true)]
public class IndexModel(IProductApiClient productApiClient) : BasePageModel
{
    [BindNever]
    public GetProduct.Response? Product { get; private set; }

    [BindNever]
    public GetProductRatings.Response? Ratings { get; private set; }

    #region Bind Properties
    public Guid ProductId { get; set; }
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 5;
    #endregion

    public async Task<IActionResult> OnGetAsync()
    {
        Product = await productApiClient.GetProductByIdAsync(new GetProduct.Request(ProductId));

        if (Product == null)
        {
            return NotFound();
        }

        Ratings = await productApiClient.GetProductRatingsAsync(new GetProductRatings.Request(ProductId, PageIndex, PageSize));

        return Page();
    }
}
