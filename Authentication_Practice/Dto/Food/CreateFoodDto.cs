using Authentication_Practice.Dto.Ingredient;
using System.ComponentModel.DataAnnotations;

namespace Authentication_Practice.Dto.Food
{
    public class CreateFoodDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "نام حداقل باید 2 کاراکتر باشد")]
        [MaxLength(50, ErrorMessage = "نام حداکثر می تواند 50 کاراکتر باشد")]
        public string Name { get; set; } = string.Empty;

        public string? Recipe { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        public IFormFile? Image { get; set; }
    }
}
