using Authentication_Practice.Dto.Food;
using Authentication_Practice.Dto.Ingredient;
using Authentication_Practice.Dto.IngredientsOfFood;
using Authentication_Practice.Models.Food;
using Authentication_Practice.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Practice.Services.FoodService
{
    public class FoodService : IFoodService
    {
        private readonly IGenericRepository<Food> _repository;

        public FoodService(IGenericRepository<Food> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<FoodDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync(
                f => new FoodDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    CategoryId = f.CategoryId,
                    CategoryName = f.Category != null ? f.Category.Name : null,
                });
        }

        public async Task<FoodDto?> GetByIdAsync(Guid Id)
        {
            //var data = await _repository.GetEntityById(Id)
            //    ?? throw new KeyNotFoundException("غذای مورد نظر پیدا نشد");

            var data = await _repository.GetQuery()
                .Include(f => f.Category)
                //.Include(f => f.IngredientsOfFood)
                    //.ThenInclude(f => f.Ingredient)
                
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == Id)
                ?? throw new KeyNotFoundException("غذای مورد نظر پیدا نشد");

            return new FoodDto
            {
                Id = data.Id,
                Name = data.Name,
                CategoryId = data.CategoryId,
                CategoryName = data.Category != null ? data.Category.Name : null,
                //Ingredients = data.IngredientsOfFood?.Select(f => new IngredientsOfFoodDto
                //{
                //    Id = f.Id,
                //    IngredientId = f.IngredientId,
                //    IngredientName = f.Ingredient != null ? f.Ingredient.Name : null,
                //    Value = f.Value,
                //}).ToList(),
            };
        }

        public async Task<FoodDto> CreateAsync(CreateFoodDto dto)
        {
            var isExist = await _repository.GetQuery()
                .AnyAsync(f => f.Name == dto.Name);

            if (isExist)
                throw new InvalidOperationException("این غذا از قبل وجود دارد");

            var food = new Food
            {
                Name = dto.Name,
                CategoryId = dto.CategoryId,
            };

            await _repository.AddEntity(food);
            await _repository.SaveAsync();

            return new FoodDto
            {
                Id = food.Id,
                Name = food.Name,
                CategoryId = food.CategoryId,
                CategoryName = food.Category != null ? food.Category.Name : null,
            };
        }

        public async Task UpdateAsync(Guid Id, UpdateFoodDto dto)
        {
            var data = await _repository.GetEntityById(Id)
                ?? throw new KeyNotFoundException("غذای مورد نظر پیدا نشد");

            data.Name = dto.Name;
            data.CategoryId = dto.CategoryId;
            //data.Ingredients = dto.Ingredients;

            _repository.EditEntity(data);
            await _repository.SaveAsync();
        }

        public async Task DeleteAsync(Guid Id)
        {
            var data = await _repository.GetEntityById(Id)
                ?? throw new KeyNotFoundException("غذای مورد نظر پیدا نشد");

            await _repository.DeletePermanent(data);
            await _repository.SaveAsync();
        }
        
    }
}
