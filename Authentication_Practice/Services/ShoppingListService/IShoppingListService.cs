using Authentication_Practice.Dto.ShoppingListItem;

namespace Authentication_Practice.Services.ShoppingListService
{
    public interface IShoppingListService
    {
        Task AddAsync(AddShoppingListDto dto);

        Task<List<ShoppingListDto>> GetAllAsync();

        Task UpdateAsync(Guid id, UpdateShoppingListDto dto);

        Task DeleteAsync(Guid id);
        Task DeleteAllAsync();
        Task DeletePurchasedAsync();
    }
}
