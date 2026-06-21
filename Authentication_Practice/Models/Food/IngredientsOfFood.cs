namespace Authentication_Practice.Models.Food
{
    public class IngredientsOfFood : BaseEntity
    {
        public Guid FoodId { get; set; }
        public Guid IngredientId { get; set; }
        public Food? Food { get; set; }
        public Ingredient? Ingredient { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
