using Authentication_Practice.Dto.Category;

namespace Authentication_Practice.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<IEnumerable<CategoryDto>> FilterCategoriesAsync(FilterCatgoryDto filter);
        Task<CategoryDto?> GetByIdAsync(Guid id);
        Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
        Task UpdateAsync(Guid Id, UpdateCategoryDto dto);
        Task DeleteAsync(Guid Id);
    }
}
