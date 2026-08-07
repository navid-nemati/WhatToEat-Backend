namespace Authentication_Practice.Models.Food
{
    public class FoodItem : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public string? Recipe { get; set; }
        public Category? Category { get; set; }
        public ICollection<IngredientsOfFood>? IngredientsOfFood { get; set; } = new List<IngredientsOfFood>();
        public string? ImagePath { get; set; }
    }
}
