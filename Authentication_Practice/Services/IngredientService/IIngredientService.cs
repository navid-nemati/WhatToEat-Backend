using Authentication_Practice.Dto.Ingredient;

namespace Authentication_Practice.Services.IngredientService
{
    public interface IIngredientService
    {
        Task<IEnumerable<IngredientDto>> GetAllAsync();
        Task<IEnumerable<IngredientDto>> FilterIngredientsAsync(FilterIngredientDto filter);
        //Task AddIngredientToFood(Guid Id, UpdateIngredientDto dto);
        Task<IngredientDto?> GetByIdAsync(Guid Id);
        Task<IngredientDto> CreateAsync(CreateIngredientDto dto);
        Task UpdateAsync(Guid Id, UpdateIngredientDto dto);
        Task DeleteAsync(Guid Id);
    }
}
