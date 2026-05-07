namespace NashFridayStore.StoreFront.Services.Categories;

public interface ICategoryApiClient
{
    Task<GetCategories.Response> GetCategoriesAsync(GetCategories.Request request);
}
