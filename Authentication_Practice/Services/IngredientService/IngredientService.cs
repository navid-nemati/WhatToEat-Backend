using Authentication_Practice.Dto.Ingredient;
using Authentication_Practice.Models.Food;
using Authentication_Practice.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Practice.Services.IngredientService
{
    public class IngredientService : IIngredientService
    {
        private readonly IGenericRepository<Ingredient> _repository;
        private readonly IGenericRepository<IngredientsOfFood> _IngredientsOfFoodRepo;

        public IngredientService(
            IGenericRepository<Ingredient> repository,
            IGenericRepository<IngredientsOfFood> IngredientsOfFoodRepo
            )
        {
            _repository = repository;
            _IngredientsOfFoodRepo = IngredientsOfFoodRepo;
        }

        public async Task<IEnumerable<IngredientDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync(i => new IngredientDto
            {
                Id = i.Id,
                Name = i.Name
            });
        }

        public async Task<IEnumerable<IngredientDto>> FilterIngredientsAsync(FilterIngredientDto filter)
        {
            var query = _repository.GetQuery();

            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(i => i.Name.Contains(filter.Name));

            var ingredients = await query.ToListAsync();

            var ingredientDtos = ingredients.Select(i => new IngredientDto
            {
                Id = i.Id, // فرض می‌کنیم Ingredient یک پراپرتی Id دارد
                Name = i.Name // فرض می‌کنیم Ingredient یک پراپرتی Name دارد
                              // سایر پراپرتی‌های IngredientDto را اینجا مپ کنید
            }).ToList(); // .ToList() را اینجا فراخوانی کنید تا IEnumerable<IngredientDto> را بدست آورید

            return ingredientDtos;
        }

        public async Task<IngredientDto?> GetByIdAsync(Guid Id)
        {
            var data = await _repository.GetEntityById(Id)
                ?? throw new KeyNotFoundException("ماده اولیه مورد نظر پیدا نشد");

            return new IngredientDto
            {
                Id = data.Id,
                Name = data.Name,
            };
        }

        public async Task<IngredientDto> CreateAsync(CreateIngredientDto dto)
        {
            var isExist = await _repository.GetQuery()
                .AnyAsync(i => i.Name == dto.Name);

            if (isExist)
                throw new InvalidOperationException("این ماده اولیه از قبل وجود دارد");

            var ingredient = new Ingredient
            {
                Name = dto.Name,
            };

            await _repository.AddEntity(ingredient);
            await _repository.SaveAsync();

            return new IngredientDto
            {
                Id = ingredient.Id,
                Name = dto.Name,
            };
        }

        public async Task UpdateAsync(Guid Id, UpdateIngredientDto dto)
        {
            var data = await _repository.GetEntityById(Id)
                ?? throw new KeyNotFoundException("ماده اولیه مورد نظر پیدا نشد");

            data.Name = dto.Name;

            _repository.EditEntity(data);
            await _repository.SaveAsync();
        }

        public async Task DeleteAsync(Guid Id)
        {
            var data = await _repository.GetEntityById(Id)
                ?? throw new KeyNotFoundException("ماده اولیه مورد نظر پیدا نشد");

            var isRelated = await _IngredientsOfFoodRepo.GetQuery()
                .AnyAsync(i => i.IngredientId == Id);

            if (isRelated)
                throw new InvalidOperationException("این آیتم را نمی توانید حذف کنید چون غذا یا غذاهایی شامل این ماده اولیه هستند");

            await _repository.DeletePermanent(data);
            await _repository.SaveAsync();
        }
    }
}
