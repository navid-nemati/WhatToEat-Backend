using Authentication_Practice.Dto.Ingredient;
using Authentication_Practice.Dto.IngredientsOfFood;
using Authentication_Practice.Models.Food;
using System.ComponentModel.DataAnnotations;

namespace Authentication_Practice.Dto.Food
{
    public class FoodDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; } = string.Empty;
        //public ICollection<IngredientsOfFoodDto>? Ingredients { get; set; } = new List<IngredientsOfFoodDto>();
    }
}
