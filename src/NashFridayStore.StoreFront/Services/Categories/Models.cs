namespace NashFridayStore.StoreFront.Services.Categories;

public record CategoryDto(Guid Id, string Name, string Description);

public record GetAllCategoriesResponse(List<CategoryDto> Items);
