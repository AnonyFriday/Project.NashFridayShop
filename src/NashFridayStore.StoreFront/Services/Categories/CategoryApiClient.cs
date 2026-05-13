namespace NashFridayStore.StoreFront.Services.Categories;

public class CategoryApiClient : ICategoryApiClient
{
    private readonly BaseApiClient _apiClient;

    public CategoryApiClient(BaseApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetCategories.Response> GetCategoriesAsync(GetCategories.Request request)
    {
        var queryParams = new List<string>();
        if (!string.IsNullOrWhiteSpace(request.SearchName))
        {
            queryParams.Add($"searchName={request.SearchName}");
        }


        if (request.PageIndex.HasValue)
        {
            queryParams.Add($"pageIndex={request.PageIndex.Value}");
        }


        if (request.PageSize.HasValue)
        {
            queryParams.Add($"pageSize={request.PageSize.Value}");
        }


        if (request.IsAll.HasValue)
        {

            queryParams.Add($"isAll={request.IsAll.Value.ToString().ToLower()}");
        }


        string queryString = queryParams.Any() ? "?" + string.Join("&", queryParams) : "";

        return await _apiClient.GetAsync<GetCategories.Response>($"api/all/categories{queryString}")
               ?? new GetCategories.Response(new(), 0, 0, 0);
    }
}
