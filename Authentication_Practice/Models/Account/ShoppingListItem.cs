using Authentication_Practice.Models.Food;

namespace Authentication_Practice.Models.Account
{
    public class ShoppingListItem : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public Guid FoodId { get; set; }
        public Guid IngredientId { get; set; }
        public string? Value { get; set; }
        public bool IsPurchased { get; set; } = false;
        public AppUser? User { get; set; }
        public FoodItem? Food { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}
