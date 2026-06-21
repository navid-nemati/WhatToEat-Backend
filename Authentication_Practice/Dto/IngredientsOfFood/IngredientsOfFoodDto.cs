namespace Authentication_Practice.Dto.IngredientsOfFood
{
    public class IngredientsOfFoodDto
    {
        public Guid Id { get; set; }
        public Guid IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
