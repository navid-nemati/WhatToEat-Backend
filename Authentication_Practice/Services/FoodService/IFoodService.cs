using Authentication_Practice.Dto.Food;
using Authentication_Practice.Dto.Ingredient;

namespace Authentication_Practice.Services.FoodService
{
    public interface IFoodService
    {
        Task<IEnumerable<FoodDto>> GetAllAsync();
        Task<FoodDto?> GetByIdAsync(Guid id);
        Task<FoodDto> CreateAsync(CreateFoodDto dto);
        Task UpdateAsync(Guid Id, UpdateFoodDto dto);
        Task DeleteAsync(Guid id);
    }
}
