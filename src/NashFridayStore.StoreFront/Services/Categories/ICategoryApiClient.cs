namespace NashFridayStore.StoreFront.Services.Categories;

public interface ICategoryApiClient
{
    Task<GetAllCategoriesResponse?> GetAllCategoriesAsync();
}
