namespace Authentication_Practice.Dto.ShoppingListItem
{
    public class ShoppingListDto
    {
        public Guid Id { get; set; }

        public Guid FoodId { get; set; }

        public Guid IngredientId { get; set; }

        public string IngredientName { get; set; } = string.Empty;
        public string FoodName { get; set; } = string.Empty;

        public string? Value { get; set; }

        public bool IsPurchased { get; set; }
    }
}
