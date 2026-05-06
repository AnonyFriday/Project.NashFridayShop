using Microsoft.AspNetCore.Mvc;
using NashFridayStore.StoreFront.Services.Categories;

namespace NashFridayStore.StoreFront.Pages.Shared.Components.CategoryNavigation;

public class CategoryNavigationViewComponent(ICategoryApiClient categoryApiClient) : ViewComponent
{
    public CategoryNavigationVM CategoryNavigationVM { get; set; } = new();

    public async Task<IViewComponentResult> InvokeAsync()
    {
        try
        {
            GetAllCategoriesResponse? response = await categoryApiClient.GetAllCategoriesAsync();
            CategoryNavigationVM.Categories = response?.Items?
                .Select(c => new CategoryNavigationVM.Category(c.Id.ToString(), c.Name))
                .ToList() ?? [];
        }
        catch
        {
            CategoryNavigationVM.Categories = [];
        }

        return View(CategoryNavigationVM);
    }
}
