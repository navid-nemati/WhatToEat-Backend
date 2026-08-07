namespace Authentication_Practice.Models.Food
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<FoodItem> Foods { get; set; } = new List<FoodItem>();
    }
}
