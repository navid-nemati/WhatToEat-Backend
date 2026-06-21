using System.ComponentModel.DataAnnotations;

namespace Authentication_Practice.Models.Food
{
    public class Food : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<IngredientsOfFood>? IngredientsOfFood { get; set; } = new List<IngredientsOfFood>();
    }
}
