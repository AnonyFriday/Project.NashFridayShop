using Microsoft.AspNetCore.Mvc;
using NashFridayStore.StoreFront.Services.Categories;

namespace NashFridayStore.StoreFront.Pages.Shared.Components.CategoryNavigation;

public class CategoryNavigationViewComponent : ViewComponent
{
    private readonly ICategoryApiClient _categoryApiClient;

    public CategoryNavigationViewComponent(ICategoryApiClient categoryApiClient)
    {
        _categoryApiClient = categoryApiClient;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        try
        {
            GetAllCategoriesResponse? response = await _categoryApiClient.GetAllCategoriesAsync();
            return View(response?.Items ?? new List<CategoryDto>());
        }
        catch
        {
            return View(new List<CategoryDto>());
        }
    }
}
