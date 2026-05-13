using Microsoft.AspNetCore.Mvc;
using NashFridayStore.StoreFront.Services.Categories;

namespace NashFridayStore.StoreFront.Pages.Shared.Components.CategoryNavigation;

public class CategoryNavigationViewComponent(ICategoryApiClient categoryApiClient) : ViewComponent
{
    public CategoryNavigationResponseVM CategoryNavigationResponseVM { get; set; } = new();

    public async Task<IViewComponentResult> InvokeAsync()
    {
        try
        {
            GetCategories.Response? response = await categoryApiClient.GetCategoriesAsync(new GetCategories.Request(IsAll: true));
            CategoryNavigationResponseVM = new CategoryNavigationResponseVM
            {
                Categories = response?.Items?
                    .Select(c => new CategoryNavigationResponseVM.Category(c.Id.ToString(), c.Name))
                    .ToList() ?? []
            };
        }
        catch
        {
            CategoryNavigationResponseVM = new CategoryNavigationResponseVM { Categories = [] };
        }

        return View(CategoryNavigationResponseVM);
    }
}
