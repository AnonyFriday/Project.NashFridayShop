using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NashFridayStore.StoreFront.Services.Cart;
using NashFridayStore.StoreFront.Services.Products;

namespace NashFridayStore.StoreFront.Pages.Products.Details;

[BindProperties(SupportsGet = true)]
public class IndexModel(
    IProductApiClient productApiClient,
    ICartApiClient cartApiClient) : BasePageModel
{
    [BindNever]
    public GetProduct.Response? Product { get; private set; }

    [BindNever]
    public GetProductRatings.Response? Ratings { get; private set; }

    #region Bind Properties
    public Guid ProductId { get; set; }
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 5;

    [BindProperty]
    public int QuantityToAdd { get; set; } = 1;

    [BindProperty]
    public int Stars { get; set; } = 5;

    [BindProperty]
    public string? Comment { get; set; }
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

    public async Task<IActionResult> OnPostPostRatingAsync()
    {
        if (Stars < 1 || Stars > 5)
        {
            ModelState.AddModelError("Stars", "Please select a star rating between 1 and 5.");
        }

        if (!ModelState.IsValid)
        {
            return await OnGetAsync();
        }

        await productApiClient.PostProductRatingAsync(new PostProductRating.Request(ProductId, Stars, Comment));

        // refresh HTML page but does not refresh a whole browser
        if (Request.Headers.ContainsKey("HX-Request"))
        {
            TriggerToast("Thank you for your feedback!", "success");
            Response.Headers.Append("HX-Refresh", "true");
            return Content("");
        }

        return RedirectToPage(new { ProductId });
    }

    public async Task<IActionResult> OnPostAddToCartAsync()
    {
        // Make sure to always get a the lastest information from product
        GetProduct.Response? product = await productApiClient.GetProductByIdAsync(new GetProduct.Request(ProductId));
        if (product == null)
        {
            return NotFound();
        }

        await cartApiClient.CreateOrAddItemAsync(new CreateOrAddItemToCart.Request(
            ProductId: ProductId,
            Quantity: QuantityToAdd,
            ProductName: product.Name,
            ImageUrl: product.ImageUrl,
            Price: product.PriceUsd,
            CategoryId: product.CategoryId,
            CategoryName: product.CategoryName
        ));

        if (Request.Headers.ContainsKey("HX-Request"))
        {
            TriggerCartUpdateIcon();
            TriggerToast($"{product.Name} added to bag!", "success");

            return new StatusCodeResult(204);
        }

        return RedirectToPage(new { ProductId });
    }
}
