using Authentication_Practice.Data;
using Authentication_Practice.Dto.ShoppingListItem;
using Authentication_Practice.Models.Account;
using Authentication_Practice.Models.Food;
using Authentication_Practice.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Authentication_Practice.Services.ShoppingListService
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly IGenericRepository<ShoppingListItem> _shoppingListRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IGenericRepository<IngredientsOfFood> _ingredientsOfFoodRepo;

        public ShoppingListService(
            IGenericRepository<ShoppingListItem> shoppingListRepository,
            IHttpContextAccessor httpContextAccessor,
            IGenericRepository<IngredientsOfFood> ingredientsOfFoodRepo)
        {
            _shoppingListRepository = shoppingListRepository;
            _httpContextAccessor = httpContextAccessor;
            _ingredientsOfFoodRepo = ingredientsOfFoodRepo;
        }

        private string GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("کاربر احراز هویت نشده است.");

            return userId;
        }

        public async Task AddAsync(AddShoppingListDto dto)
        {
            var userId = GetUserId();

            if (string.IsNullOrEmpty(userId))
                throw new Exception("UserId is null");

            var isExist = await _shoppingListRepository.GetQuery()
                .AnyAsync(i => 
                i.UserId == userId && 
                i.IngredientId == dto.IngredientId &&
                i.FoodId == dto.FoodId &&
                !i.IsPurchased);

            if (isExist)
                throw new InvalidOperationException("این ماده از قبل در لیست شما وجود دارد");

            var ingredientOfFood = await _ingredientsOfFoodRepo.GetQuery()
                .FirstOrDefaultAsync(i => 
                i.FoodId == dto.FoodId &&
                i.IngredientId == dto.IngredientId);

            if (ingredientOfFood == null)
                throw new KeyNotFoundException("ماده اولیه مربوط به غذا پیدا نشد");

            var shoppingListItem = new ShoppingListItem
            {
                UserId = userId,
                IngredientId = dto.IngredientId,
                FoodId = dto.FoodId,
                Value = ingredientOfFood.Value,
                //Value = dto.Value,
            };

            await _shoppingListRepository.AddEntity(shoppingListItem);
            await _shoppingListRepository.SaveAsync();

            //return new ShoppingListDto
            //{
            //    Id = ShoppingListItem.Id,
            //    IngredientId = ShoppingListItem.IngredientId,
            //    IngredientName = ShoppingListItem.Ingredient.Name,
            //    Value = ShoppingListItem.Value,
            //    IsPurchased = ShoppingListItem.IsPurchased,
            //};
        }

        public async Task<List<ShoppingListDto>> GetAllAsync()
        {
            var userId = GetUserId();

            var items = await _shoppingListRepository.GetQuery()
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Include(x => x.Ingredient)
                .Include(x => x.Food)
                .Select(i => new ShoppingListDto
                {
                    Id = i.Id,
                    IngredientId = i.IngredientId,
                    IngredientName = i.Ingredient.Name,
                    FoodName = i.Food.Name,
                    Value = i.Value,
                    IsPurchased = i.IsPurchased,
                })
                .ToListAsync();

            return items;
        }

        public async Task UpdateAsync(Guid id, UpdateShoppingListDto dto)
        {
            var userId = GetUserId();

            var item = await _shoppingListRepository.GetQuery()
                .FirstOrDefaultAsync(i => i.Id == id &&
                    i.UserId == userId);

            if (item == null)
            {
                throw new KeyNotFoundException("آیتم پیدا نشد");
            }

            if (dto.Value != null)
                item.Value = dto.Value;

            if (dto.IsPurchased.HasValue)
                item.IsPurchased = dto.IsPurchased.Value;

            //_repository.EditEntity(item);
            await _shoppingListRepository.SaveAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var userId = GetUserId();

            var item = await _shoppingListRepository.GetQuery()
                .FirstOrDefaultAsync(i => i.UserId == userId &&
                i.Id == id);

            if (item == null)
            {
                throw new KeyNotFoundException("آیتم پیدا نشد");
            }

            await _shoppingListRepository.DeletePermanent(item);
            await _shoppingListRepository.SaveAsync();
        }

        public async Task DeleteAllAsync()
        {
            var userId = GetUserId();

            var items = await _shoppingListRepository.GetQuery()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if (!items.Any())
                return;

            _shoppingListRepository.DeletePermanentEntities(items);

            await _shoppingListRepository.SaveAsync();
        }

        public async Task DeletePurchasedAsync()
        {
            var userId = GetUserId();

            var items = await _shoppingListRepository.GetQuery()
                .Where(x =>
                    x.UserId == userId &&
                    x.IsPurchased)
                .ToListAsync();

            if (!items.Any())
                return;

            _shoppingListRepository.DeletePermanentEntities(items);

            await _shoppingListRepository.SaveAsync();
        }
    }
}
