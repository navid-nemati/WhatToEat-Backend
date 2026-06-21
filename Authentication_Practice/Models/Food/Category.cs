namespace Authentication_Practice.Models.Food
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Food> Foods { get; set; } = new List<Food>();
    }
}
