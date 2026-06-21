using Authentication_Practice.Dto.IngredientsOfFood;
using Authentication_Practice.Models.Food;

namespace Authentication_Practice.Services.IngredientsOfFoodService
{
    public interface IIngredientsOfFoodService
    {
        Task<IEnumerable<IngredientsOfFoodDto>> GetAllIngredientsOfFoodByFoodIdAync(Guid FoodId);
        Task<IngredientsOfFoodDto> CreateIngredientsOfFoodAsync(CreateIngredientsOfFoodDto dto);
        Task UpdateIngredientsOfFood(Guid Id, UpdateIngredientsOfFoodDto dto);
        Task DeleteIngredientsOfFood(Guid Id);
    }
}
