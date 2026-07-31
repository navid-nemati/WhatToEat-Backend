using System.ComponentModel.DataAnnotations;

namespace Authentication_Practice.Dto.IngredientsOfFood
{
    public class CreateIngredientsOfFoodDto
    {
        public Guid FoodId { get; set; }
        public Guid IngredientId { get; set; }
        [Required(ErrorMessage = "لطفا مقدار ماده اولیه را وارد کنید")]
        public string Value { get; set; } = string.Empty;   
    }
}
