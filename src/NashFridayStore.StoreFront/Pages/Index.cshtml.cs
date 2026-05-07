using Microsoft.AspNetCore.Mvc;
using NashFridayStore.StoreFront.Pages.Shared.PartialViews.SectionHeader;
using NashFridayStore.StoreFront.Services.Products;

namespace NashFridayStore.StoreFront.Pages;

public class IndexModel(IProductApiClient productApiClient) : BasePageModel
{
    private const int _topRatedCount = 10;

    #region Bind Properties

    [BindProperty(SupportsGet = true)]
    public IList<GetTopRatedProducts.ProductItem> TopProducts { get; set; }
    public SectionHeaderVM TopRatedSectionHeader { get; init; } = new SectionHeaderVM("Top Rated", "Discover our most loved and highest rated items.");

    #endregion

    public async Task OnGetAsync()
    {
        GetTopRatedProducts.Response? response = await productApiClient.GetTopRatedProductsAsync(new GetTopRatedProducts.Request(_topRatedCount));
        TopProducts = response?.Items ?? [];
    }
}
