using Authentication_Practice.Models.Account;

namespace Authentication_Practice.Models.Food
{
    public class Ingredient : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<IngredientsOfFood> IngredientsOfFood { get; set; }
            = new List<IngredientsOfFood>();
        public ICollection<ShoppingListItem> ShoppingListItems { get; set; }
            = new List<ShoppingListItem>();
    }
}
