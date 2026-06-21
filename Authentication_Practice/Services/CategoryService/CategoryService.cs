using Authentication_Practice.Dto.Category;
using Authentication_Practice.Models.Food;
using Authentication_Practice.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Practice.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _repository;
        private readonly IGenericRepository<Food> _FoodRepo;

        public CategoryService(IGenericRepository<Category> repository,
            IGenericRepository<Food> FoodRepo)
        {
            _repository = repository;
            _FoodRepo = FoodRepo;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync(
                c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                });
        }

        public async Task<IEnumerable<CategoryDto>> FilterCategoriesAsync(FilterCatgoryDto filter)
        {
            var query = _repository.GetQuery();

            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(c => c.Name.Contains(filter.Name));

            var categories = await query.ToListAsync();

            var categoryDtos = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
            });

            return categoryDtos;
        }

        public async Task<CategoryDto?> GetByIdAsync(Guid id)
        {
            var data = await _repository.GetEntityById(id)
                ?? throw new KeyNotFoundException("دسته بندی پیدا نشد");

            return new CategoryDto
            {
                Id = data.Id,
                Name = data.Name,
            };
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            var isExist = await _repository.GetQuery()
                .AnyAsync(c => c.Name == dto.Name);

            if (isExist)
                throw new InvalidOperationException("این دسته بندی از قبل وجود دارد");

            var category = new Category { Name = dto.Name };

            await _repository.AddEntity(category);
            await _repository.SaveAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

        public async Task UpdateAsync(Guid Id, UpdateCategoryDto dto)
        {
            var data = await _repository.GetEntityById(Id)
                ?? throw new KeyNotFoundException("دسته بندی مورد نظر پیدا نشد");

            data.Name = dto.Name;
            _repository.EditEntity(data);
            await _repository.SaveAsync();
        }

        public async Task DeleteAsync(Guid Id)
        {
            var data = await _repository.GetEntityById(Id)
                ?? throw new KeyNotFoundException("دسته بندی مورد نظر پیدا نشد");

            var isRelated = await _FoodRepo.GetQuery()
                .AnyAsync(i => i.CategoryId == Id);

            if (isRelated)
                throw new InvalidOperationException("این آیتم را نمی توانید حذف کنید چون غذا یا غذاهایی شامل این دسته بندی هستند");

            await _repository.DeletePermanent(data);
            await _repository.SaveAsync();
        }
    }
}
