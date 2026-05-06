namespace NashFridayStore.StoreFront.Services.Categories;

public class CategoryApiClient : ICategoryApiClient
{
    private readonly BaseApiClient _apiClient;

    public CategoryApiClient(BaseApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetAllCategoriesResponse?> GetAllCategoriesAsync()
    {
        return await _apiClient.GetAsync<GetAllCategoriesResponse>("api/all/categories?isAll=true");
    }
}
