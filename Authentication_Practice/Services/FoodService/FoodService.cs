using Authentication_Practice.Dto.Food;
using Authentication_Practice.Dto.Ingredient;
using Authentication_Practice.Dto.IngredientsOfFood;
using Authentication_Practice.Models.Food;
using Authentication_Practice.Repositories;
using Authentication_Practice.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Practice.Services.FoodService
{
    public class FoodService : IFoodService
    {
        private readonly IGenericRepository<Food> _repository;
        private readonly IFileStorageService _fileStorage;

        private const string FoodImagesFolder = "uploads/foods";

        public FoodService(
            IGenericRepository<Food> repository,
            IFileStorageService fileStorage)
        {
            _repository = repository;
            _fileStorage = fileStorage;
        }

        public async Task<IEnumerable<FoodDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync(
                food => new FoodDto
                {
                    Id = food.Id,
                    Name = food.Name,
                    CategoryId = food.CategoryId,
                    CategoryName = food.Category != null ? food.Category.Name : null,
                    ImagePath = food.ImagePath
                });
        }

        public async Task<FoodDto?> GetByIdAsync(Guid id)
        {
            var data = await _repository.GetQuery()
            .AsNoTracking()
            .Where(food => food.Id == id)
            .Select(food => new FoodDto
            {
                Id = food.Id,
                Name = food.Name,
                CategoryId = food.CategoryId,
                Recipe = food.Recipe,
                CategoryName = food.Category != null
                    ? food.Category.Name
                    : null,
                ImagePath = food.ImagePath
            })
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException(
                "غذای مورد نظر پیدا نشد");

            return data;

            //var data = await _repository.GetQuery()
            //    .Include(f => f.Category)
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(f => f.Id == Id)
            //    ?? throw new KeyNotFoundException("غذای مورد نظر پیدا نشد");

            //return new FoodDto
            //{
            //    Id = data.Id,
            //    Name = data.Name,
            //    CategoryId = data.CategoryId,
            //    CategoryName = data.Category != null ? data.Category.Name : null,
            //    Recipe = data.Recipe,
            //    ImagePath = data.ImagePath,
            //};
        }

        public async Task<FoodDto> CreateAsync(
            CreateFoodDto dto,
            CancellationToken cancellationToken = default)
        {
            var normalizedName = dto.Name.Trim();

            if (dto.CategoryId == Guid.Empty)
            {
                throw new InvalidOperationException(
                    "لطفاً یک دسته‌بندی انتخاب کنید");
            }

            var isExist = await _repository.GetQuery()
                .AnyAsync(f => f.Name == normalizedName,
                cancellationToken);

            if (isExist)
                throw new InvalidOperationException("این غذا از قبل وجود دارد");

            string? ImagePath = null;

            try
            {
                if (dto.Image is not null)
                {
                    ImagePath = await _fileStorage.SaveImageAsync(
                        dto.Image,
                        FoodImagesFolder,
                        cancellationToken);


                }

                var food = new Food
                {
                    Name = dto.Name,
                    CategoryId = dto.CategoryId,
                    Recipe = dto.Recipe,

                    ImagePath = ImagePath
                };

                await _repository.AddEntity(food);
                await _repository.SaveAsync();

                return new FoodDto
                {
                    Id = food.Id,
                    Name = food.Name,
                    CategoryId = food.CategoryId,
                    //CategoryName = food.Category != null ? food.Category.Name : null,
                    Recipe = food.Recipe,
                    ImagePath = food.ImagePath,
                };
            }
            catch
            {
                if(ImagePath is not null)
                {
                    await _fileStorage.DeleteAsync(
                        ImagePath,
                        cancellationToken);
                }

                throw;
            }

            //var food = new Food
            //{
            //    Name = dto.Name,
            //    CategoryId = dto.CategoryId,
            //    Recipe = dto.Recipe,
            //};

            //await _repository.AddEntity(food);
            //await _repository.SaveAsync();

            //return new FoodDto
            //{
            //    Id = food.Id,
            //    Name = food.Name,
            //    CategoryId = food.CategoryId,
            //    CategoryName = food.Category != null ? food.Category.Name : null,
            //    Recipe = food.Recipe,
            //};
        }

        public async Task UpdateAsync(
            Guid id, 
            UpdateFoodDto dto,
            CancellationToken cancellationToken = default)
        {
            var data = await _repository.GetEntityById(id)
                ?? throw new KeyNotFoundException("غذای مورد نظر پیدا نشد");

            var normalizedName = dto.Name.Trim();

            if (dto.CategoryId == Guid.Empty)
            {
                throw new InvalidOperationException(
                    "لطفاً یک دسته‌بندی انتخاب کنید");
            }

            var duplicateNameExists = await _repository.GetQuery()
            .AnyAsync(
                food =>
                    food.Id != id &&
                    food.Name == normalizedName,
                cancellationToken);

            if (duplicateNameExists)
            {
                throw new InvalidOperationException(
                    "غذای دیگری با این نام وجود دارد");
            }

            var oldImagePath = data.ImagePath;
            string? newImagePath = null;

            try
            {
                if (dto.Image is not null)
                {
                    newImagePath = await _fileStorage.SaveImageAsync(
                        dto.Image,
                        FoodImagesFolder,
                        cancellationToken);

                    data.ImagePath = newImagePath;
                }
                else if (dto.RemoveImage)
                    data.ImagePath = null;

                data.Name = normalizedName;
                data.CategoryId = dto.CategoryId;
                data.Recipe = dto.Recipe;

                _repository.EditEntity(data);

                await _repository.SaveAsync();

                var ImageWasReplaced = newImagePath is not null;
                var ImageWasRemoved = dto.RemoveImage && dto.Image is null;

                if ((ImageWasReplaced || ImageWasRemoved) &&
                    oldImagePath is not null)
                {
                    await _fileStorage.DeleteAsync(
                        oldImagePath,
                        cancellationToken);
                }
            }
            catch
            {
                if (newImagePath is not null)
                {
                    await _fileStorage.DeleteAsync(
                        newImagePath,
                        cancellationToken);
                }

                throw;
            }

            //data.Name = dto.Name;
            //data.CategoryId = dto.CategoryId;
            //data.Recipe = dto.Recipe;

            //_repository.EditEntity(data);
            //await _repository.SaveAsync();
        }

        public async Task DeleteAsync(
            Guid Id,
            CancellationToken cancellationToken = default)
        {
            var data = await _repository.GetEntityById(Id)
                ?? throw new KeyNotFoundException("غذای مورد نظر پیدا نشد");

            var imagePath = data.ImagePath;

            await _repository.DeletePermanent(data);
            await _repository.SaveAsync();

            if (imagePath is not null)
            {
                await _fileStorage.DeleteAsync(
                    imagePath,
                    cancellationToken);
            }
        }
        
    }
}
