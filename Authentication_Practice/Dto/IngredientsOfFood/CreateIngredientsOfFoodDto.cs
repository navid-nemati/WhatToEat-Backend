namespace Authentication_Practice.Dto.IngredientsOfFood
{
    public class CreateIngredientsOfFoodDto
    {
        public Guid FoodId { get; set; }
        public Guid IngredientId { get; set; }
        public string Value { get; set; } = string.Empty;   
    }
}
