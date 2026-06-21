using Authentication_Practice.Dto.IngredientsOfFood;
using Authentication_Practice.Models.Food;
using Authentication_Practice.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Practice.Services.IngredientsOfFoodService
{
    public class IngredientsOfFoodService : IIngredientsOfFoodService
    {
        private readonly IGenericRepository<IngredientsOfFood> _repository;

        public IngredientsOfFoodService(IGenericRepository<IngredientsOfFood> repository)
        {
            _repository = repository;
        }

        //public async Task<IEnumerable<IngredientsOfFood>> GetAllIngredientsOfFoodByFoodIdAync(Guid FoodId)
        //{
        //    var a = await _repository.GetQuery()
        //        .AsNoTracking()
        //        .Where(i => i.Id == FoodId)
        //        .Include(c => c.Ingredient)
        //        .Select(i => new IngredientsOfFoodDto
        //        {
        //            Id = i.Id,
        //            IngredientId = i.IngredientId,
        //            IngredientName = i.Ingredient.Name,
        //            Value = i.Value,
        //        })
        //        .ToListAsync();

        //    return a;
        //}

        public async Task<IEnumerable<IngredientsOfFoodDto>> GetAllIngredientsOfFoodByFoodIdAync(Guid FoodId)
        {
            var a = await _repository.GetQuery()
                .AsNoTracking()
                .Where(i => i.FoodId == FoodId) // اصلاح: FoodId را بررسی کنید
                .Include(c => c.Ingredient)
                .Select(i => new IngredientsOfFoodDto
                {
                    Id = i.Id,
                    IngredientId = i.IngredientId,
                    IngredientName = i.Ingredient.Name, // اضافه شد
                    Value = i.Value,
                })
                .ToListAsync();

            return a; // حذف cast
        }

        public async Task<IngredientsOfFoodDto> CreateIngredientsOfFoodAsync(CreateIngredientsOfFoodDto dto)
        {
            var IngredientsOfFood = new IngredientsOfFood
            {
                FoodId = dto.FoodId,
                IngredientId = dto.IngredientId,
                Value = dto.Value,
            };

            await _repository.AddEntity(IngredientsOfFood);
            await _repository.SaveAsync();

            return new IngredientsOfFoodDto
            {
                Id = IngredientsOfFood.Id,
                IngredientId = IngredientsOfFood.IngredientId,
                //IngredientName = IngredientsOfFood.Ingredient.Name,
                Value = IngredientsOfFood.Value,
            };
        }

        public async Task UpdateIngredientsOfFood(Guid Id, UpdateIngredientsOfFoodDto dto)
        {
            var data = await _repository.GetEntityById(Id)
                ?? throw new KeyNotFoundException("نگهدارنده ماده اولیه پیدا نشد");

            data.IngredientId = dto.IngredientId;
            data.Value = dto.Value;

            _repository.EditEntity(data);
            await _repository.SaveAsync();
        }
    
        public async Task DeleteIngredientsOfFood(Guid Id)
        {
            var data = await _repository.GetEntityById(Id)
                ?? throw new KeyNotFoundException("نگهدارنده ماده اولیه پیدا نشد");

            await _repository.DeletePermanent(data);
            await _repository.SaveAsync();
        }
    }
}
