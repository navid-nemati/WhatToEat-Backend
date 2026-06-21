namespace Authentication_Practice.Dto.IngredientsOfFood
{
    public class UpdateIngredientsOfFoodDto
    {
        public Guid IngredientId { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
