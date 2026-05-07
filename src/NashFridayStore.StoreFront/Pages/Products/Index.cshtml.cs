using Microsoft.AspNetCore.Mvc;
using NashFridayStore.StoreFront.Services.Categories;
using NashFridayStore.StoreFront.Services.Products;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NashFridayStore.StoreFront.Pages.Products;

[BindProperties(SupportsGet = true)]
public class IndexModel(
    IProductApiClient productApiClient,
    ICategoryApiClient categoryApiClient) : BasePageModel
{
    [BindNever]
    public GetProducts.Response? ProductResponse { get; set; }

    [BindNever]
    public GetCategories.Response? CategoryResponse { get; set; }

    public Guid? CategoryId { get; set; }
    public string? SearchName { get; set; }
    public SortBy? SortBy { get; set; }

    public async Task OnGetAsync()
    {
        CategoryResponse = await categoryApiClient.GetCategoriesAsync(new GetCategories.Request(IsAll: true));

        var request = new GetProducts.Request(
            SearchName: string.IsNullOrWhiteSpace(SearchName) ? null : SearchName,
            CategoryId: CategoryId,
            MinPrice: null,
            MaxPrice: null,
            SortBy: SortBy,
            PageIndex: 0,
            PageSize: 10
        );

        ProductResponse = await productApiClient.GetProductsAsync(request);
    }
}
